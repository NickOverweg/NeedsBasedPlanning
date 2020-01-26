using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentInventory), typeof(GAgent))]
public class Gatherer : MonoBehaviour, IGoap
{
    public AgentInventory inventory;
    public GAgent agent;

    void Start()
    {
        inventory = gameObject.GetComponent<AgentInventory>();
        agent = gameObject.GetComponent<GAgent>();

        //Gatherer actions
        gameObject.AddComponent<GatherAction>();
        gameObject.AddComponent<StoreAction>();
        gameObject.AddComponent<GoToGatherAction>();
        gameObject.AddComponent<GoToStorageAction>();
    }

    public void ActionsFinished()
    {
        Debug.Log("<color=blue>Actions completed</color>");
    }

    public Dictionary<string, object> CreateGoalState()
    {
        Dictionary<string, object> goalState = new Dictionary<string, object>();

        goalState.Add("hasStored", true);

        return goalState;
    }

    public Dictionary<string, object> GetWorldState()
    {
        Dictionary<string, object> worldState = new Dictionary<string, object>();

        //worldState.Add("hasGathered", true);
        //worldState.Add("isAtLocation", agent.transform.position);
        worldState.Add("hasStored", false);
        
        return worldState;

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
        Debug.Log("<color=green>Plan found</color> " + actions);

        GAction[] ActionQ = actions.ToArray();

        foreach(GAction act in ActionQ)
        {
            Debug.Log(act.GetType());
        }


    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
