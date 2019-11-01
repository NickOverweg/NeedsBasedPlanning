using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GBlackBoard : MonoBehaviour
{
    private Dictionary<SourceNames, HashSet<GameObject>> allKnownSources;

    public GameObject Storage;

    //TODO: design better representation for scouting surrounding area
    private Queue<Vector3> scoutLocations;

    private HashSet<GameObject> registeredAgents;
    
    private int AgentsMaslowLevel2;
    private int AgentsMaslowLevel3;

    //survival needs
    private static int foodPerDayPerAgent = 5;
    private static int waterPerDayPerAgent = 2;
    private static int agentsPerFire = 10;
    private static int bedsPerAgent = 1;

    //safety needs
    private static int daysWorthOfConsumables = 7;
    private static int AgentsPerBuilding = 2;

    //social needs
    private static int freeHoursPerDay = 4;
    private static int socializingSpace = 1;


    

    public GBlackBoard()
    {
        allKnownSources = new Dictionary<SourceNames, HashSet<GameObject>>();
        scoutLocations = new Queue<Vector3>();
        FillScoutLocations();
    }

    public void AddSourceLocation(SourceNames dictName, GameObject sourceToAdd)
    {
        allKnownSources.TryGetValue(dictName, out var specificKnownSources);

        if (specificKnownSources != null)
            specificKnownSources.Add(sourceToAdd);
        else
        {
            HashSet<GameObject> newSet = new HashSet<GameObject>();
            newSet.Add(sourceToAdd);
            allKnownSources.Add(dictName, newSet);
        }

    }

    public GameObject RequestSourceLocation(SourceNames dictName)
    {
        GameObject sourceToReturn;

        if (allKnownSources.TryGetValue(dictName, out HashSet<GameObject> requestedSources))
        {
            sourceToReturn = requestedSources.FirstOrDefault();

            if (sourceToReturn != null)
                return sourceToReturn;
        }

        return null;
    }


    public Vector3 RequestScoutLocation
    {
        get
        {
            if (scoutLocations.Count > 0) return scoutLocations.Dequeue();
            return Vector3.zero;
        }
    }

    private void FillScoutLocations()
    {
        scoutLocations.Enqueue(new Vector3(10f, 0f, -10f));
        scoutLocations.Enqueue(new Vector3(10f, 0f, 0f));
        scoutLocations.Enqueue(new Vector3(10f, 0f, 10f));
        scoutLocations.Enqueue(new Vector3(0f, 0f, 10f));
        scoutLocations.Enqueue(new Vector3(-10f, 0f, 10f));
        scoutLocations.Enqueue(new Vector3(-10f, 0f, 0f));
        scoutLocations.Enqueue(new Vector3(-10f, 0f, -10f));
        scoutLocations.Enqueue(new Vector3(0f, 0f, -10f));
    }

    private void calculateDailyNeeds()
    {
        //registeredAgents.Count
    }


}

