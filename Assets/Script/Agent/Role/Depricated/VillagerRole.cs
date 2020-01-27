using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A general labourer class.
 * You should subclass this for specific Labourer classes and implement
 * the createGoalState() method that will populate the goal for the GOAP
 * planner.
 */

[RequireComponent(typeof(AgentInventory), typeof(GAgent))]
public class VillagerRole : MonoBehaviour, IGoap
{
    public AgentInventory inventory;
    public GAgent agent;

    public float moveSpeed = 1;

    private bool recievedWork = false;
    private bool finishedWork = false;

    void Start()
    {
        inventory = gameObject.GetComponent<AgentInventory>();
        agent = gameObject.GetComponent<GAgent>();

        
    }


    void Update()
    {

    }

    /**
	 * Key-Value data that will feed the GOAP actions and system while planning.
	 */
    public Dictionary<string, object> GetWorldState()
    {
        Dictionary<string, object> worldData = new Dictionary<string, object>();

        worldData.Add("hasWood", inventory.numWood > 0);
        worldData.Add("hasFood", inventory.numFood > 0);
        //worldData.Add("hasWater", inventory.numWater > 0);
        worldData.Add("hasTarget", agent.Target != null);
        //worldData.Add("nearTarget", agent.NearTarget);
        //worldData.Add("maslowLevel", agent.MasLevel);
        worldData.Add("hasRole", false);
        worldData.Add("RecievedWork", recievedWork);
        worldData.Add("FinishedWork", finishedWork);
        

        //worldData.Add("nearBlackBoard", IsNearBlackBoard());

        return worldData;
    }

    /**
	 * Implement in subclasses
	 */
    public Dictionary<string, object> CreateGoalState()
    {
        Dictionary<string, object> goalState = new Dictionary<string, object>();

        goalState.Add("RecievedWork", true);
        
        if (recievedWork == false) //set the target to blackboard if the agent hasnt targeted the blackboard yet.
        {
            agent.TargetBlackBoard();
        }
        else
        {
            goalState.Add("FinishedWork", true);
        }

        return goalState;
    }


    public void PlanFailed(Dictionary<string, object> failedGoal)
    {
        // what to do in case of a failed goal, how can the worldState be changed to allow for a different outcome?
        
        Debug.Log("<color=red>Planning FAILED</color> unable to find actions for:" + failedGoal);
    }

    public void PlanFound(Dictionary<string, object> goal, Queue<GAction> actions)
    {
        // Yay we found a plan for our goal
        Debug.Log("<color=green>Plan found</color> " + actions);
    }

    public void ActionsFinished()
    {
        // Everything is done, we completed our actions for this gool. Hooray!
        Debug.Log("<color=blue>Actions completed</color>");
    }

    public void PlanAborted(GAction aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.
        Debug.Log("<color=red>Plan Aborted</color> " + aborter);
    }

    public object CheckWorldState(string keyToCheck)
    {
        throw new System.NotImplementedException();
    }

    /*
    public bool IsNearBlackBoard()
    {
        
        if(agent.NearTarget && agent.Target == agent.BlackBoard.gameObject)
        {
            return true;
        }

        return false;
        
    }
    */
}
