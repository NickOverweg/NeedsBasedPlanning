using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateKeys
{
    HungerStat,
    FoodInv,
    ThirstStat,
    OwnsCampfire,
    OwnsBed,
    Location,
    WoodInv,
    LeafInv,
    GotWood,
}

public enum LocationType
{
    FoodGatherable,
    Drinkable,
    Bed,
    Forest,
    None,
}

public static class WorldState
{
    //hardcoding locations for now. needs to be reworked   //TODO: think about locations
    private static Vector3 waterLocation = new Vector3(8, 0, 12);
    private static Vector3 foodLocation = new Vector3(-10, 0, 15);
    private static Vector3 forestLocation = new Vector3(-15, 0, 0);
    private static Vector3 campLocation = new Vector3(11, 0, 0);



    public static object HungerValue(GAgent agent)
    {
        return agent.stats.hunger;
    }

    public static object FoodValue(GAgent agent)
    {
        return agent.inventory.numFood;
    }

    public static object ThirstValue(GAgent agent)
    {
        return agent.stats.thirst;
    }

    public static object CampfireValue(GAgent agent)
    {
        if (agent.stats.campfire != null) return true;
        return false;
    }

    public static object BedValue(GAgent agent)
    {
        if (agent.stats.bed != null) return true;
        return false;
    }

    public static object LocationValue(GAgent agent)
    {
        return nameof(agent.lastKnownLocation);

    }

    public static object WoodValue(GAgent agent)
    {
        return agent.inventory.numWood;
    }

    public static object LeafValue(GAgent agent)
    {
        return agent.inventory.numLeaf;
    }

    public static object getValueFromKey(string key, GAgent agent)
    {
        switch (key)
        {
            case nameof(StateKeys.FoodInv):
                return FoodValue(agent);
            case nameof(StateKeys.HungerStat):
                return HungerValue(agent);
            case nameof(StateKeys.Location):
                return LocationValue(agent);
            case nameof(StateKeys.OwnsBed):
                return BedValue(agent);
            case nameof(StateKeys.OwnsCampfire):
                return CampfireValue(agent);
            case nameof(StateKeys.WoodInv):
                return WoodValue(agent);
            case nameof(StateKeys.LeafInv):
                return LeafValue(agent);
            default:
                return null;
        }
    }

    public static Vector3 GetLocation(LocationType location, GAgent agent)
    {
        switch (location)
        {
            case LocationType.Bed:
                if(agent.stats.bed != null) return agent.stats.bed.position;
                return campLocation;
            case LocationType.Drinkable:
                return waterLocation;
            case LocationType.FoodGatherable:
                return foodLocation;
            case LocationType.Forest:
                return forestLocation;
        }


        Debug.Log("Woops!");
        return Vector3.zero;
    }
}
