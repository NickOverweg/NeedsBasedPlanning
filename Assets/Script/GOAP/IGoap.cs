using UnityEngine;
using System.Collections;

/**
 * Collect the world data for this Agent that will be
 * used for GOAP planning.
 */
using System.Collections.Generic;


/**
 * Any agent that wants to use GOAP must implement
 * this interface. It provides information to the GOAP
 * planner so it can plan what actions to use.
 * 
 * It also provides an interface for the planner to give 
 * feedback to the Agent and report success/failure.
 * 
 * Because this is responsible for creating a goal state, 
 * this can be used as the input for agent needs
 */
public interface IGoap
{

    /**
	 * The starting state of the Agent and the world.
	 * Supply what states are needed for actions to run.
	 */
    Dictionary<string, object> GetWorldState();

    /**
	 * Give the planner a new goal so it can figure out 
	 * the actions needed to fulfill it.
	 */
    Dictionary<string, object> CreateGoalState();

    /**
	 * No sequence of actions could be found for the supplied goal.
	 * You will need to try another goal
	 */
    void PlanFailed(Dictionary<string, object> FailedGoal);

    /**
	 * A plan was found for the supplied goal.
	 * These are the actions the Agent will perform, in order.
	 */
    void PlanFound(Dictionary<string, object> goal, Queue<GAction> actions);

    /**
	 * All actions are complete and the goal was reached. Hooray!
	 */
    void ActionsFinished();

    /**
	 * One of the actions caused the plan to abort.
	 * That action is returned.
	 */
    void PlanAborted(GAction aborter);

}

