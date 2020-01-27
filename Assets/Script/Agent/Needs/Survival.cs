using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentInventory), typeof(GAgent))]
public class Survival : MonoBehaviour, IGoap
{
    public AgentInventory inventory;
    public GAgent agent;
    

    void Start()
    {
        inventory = gameObject.GetComponent<AgentInventory>();
        agent = gameObject.GetComponent<GAgent>();

        
    }



    public void ActionsFinished()
    {
        Debug.Log("<color=blue>Actions completed</color>");
    }

    public Dictionary<string, object> CreateGoalState()
    {
        Dictionary<string, object> goalState = new Dictionary<string, object>();

        goalState.Add(nameof(StateKeys.HungerStat), 0);
        goalState.Add(nameof(StateKeys.ThirstStat), 0);
        goalState.Add(nameof(StateKeys.OwnsBed), true);
        goalState.Add(nameof(StateKeys.OwnsCampfire), true);

        return goalState;
    }

    public Dictionary<string, object> GetWorldState()
    {
        Dictionary<string, object> worldState = new Dictionary<string, object>();

        worldState.Add(nameof(StateKeys.HungerStat), WorldState.HungerValue(agent));
        worldState.Add(nameof(StateKeys.FoodInv), WorldState.FoodValue(agent));
        worldState.Add(nameof(StateKeys.ThirstStat), WorldState.ThirstValue(agent));
        worldState.Add(nameof(StateKeys.OwnsBed), WorldState.BedValue(agent));
        worldState.Add(nameof(StateKeys.OwnsCampfire), WorldState.CampfireValue(agent));
        worldState.Add(nameof(StateKeys.Location), WorldState.LocationValue(agent));
        worldState.Add(nameof(StateKeys.WoodInv), WorldState.WoodValue(agent));
        worldState.Add(nameof(StateKeys.LeafInv), WorldState.LeafValue(agent));

        return worldState;
    }

    public object CheckWorldState(string keyToCheck)
    {
        return WorldState.getValueFromKey(keyToCheck, agent);
    }

    public void PlanAborted(GAction aborter)
    {

        Debug.Log("Aborted on action: " + aborter.name + "could not complete.");
    }

    public void PlanFailed(Dictionary<string, object> FailedGoal)
    {
        Debug.Log("<color=red>Planning FAILED</color> unable to find actions for:" + FailedGoal);
    }

    public void PlanFound(Dictionary<string, object> goal, Queue<GAction> actions)
    {
        Debug.Log("<color=green>Plan found</color> ");
        /*
        GAction[] plan = new GAction[20];
        actions.CopyTo(plan, 0);

        foreach(GAction act in plan)
        {
            if(act != null) Debug.Log(act.GetType());
        }
        */
    }
}
