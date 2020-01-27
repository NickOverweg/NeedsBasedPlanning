using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AgentInventory), typeof(AgentStats))]
public class GAgent : MonoBehaviour
{
    private GFSM stateMachine;

    private GFSM.IGFSMState planningState; 
    private GFSM.IGFSMState performActionState;

    public GameObject Target;

    public float moveSpeed = 1000f;

    public GameObject campfire;
    public GameObject bed;

    public bool RepeatAfterFinish = false;

    public AgentInventory inventory;
    public AgentStats stats;

    public LocationType lastKnownLocation;

    private HashSet<GAction> availableActions;
    private Queue<GAction> currentActions;

    private IGoap worldDataProvider;
    private GPlanner planner;
    private GBlackBoard blackBoard;

    public GBlackBoard BlackBoard
    {
        get { return blackBoard; }
    }

    private void Start()
    {
        stateMachine = new GFSM();
        availableActions = new HashSet<GAction>();
        currentActions = new Queue<GAction>();
        planner = new GPlanner();


        //availableActions.
        //gameObject.GetComponents<GAction>()

        inventory = GetComponent<AgentInventory>();
        
        stats = GetComponent<AgentStats>();

        FindDataProvider();
        FindBlackBoard();

        CreatePlanningState();
        CreatePerformActionState();
        stateMachine.PushState(planningState);
        LoadActions();
    }

    private void Update()
    {
        stateMachine.Update(this);
    }

    public void AddAction(GAction a)
    {
        availableActions.Add(a);
    }

    public GAction GetAction(Type action)
    {
        foreach(GAction g in availableActions)
        {
            if (g.GetType().Equals(action))
                return g;
        }
        return null;
    }

    public void RemoveAction(GAction action)
    {
        availableActions.Remove(action);
    }

    private bool HasActionPlan()
    {
        return currentActions.Count > 0;
    }

    private void CreatePlanningState()
    {
        planningState = (fsm, gameObj) =>
        {
            Dictionary<string, object> worldState = worldDataProvider.GetWorldState();
            Dictionary<string, object> goalState = worldDataProvider.CreateGoalState();

            Queue<GAction> plan = planner.Plan(this, availableActions, worldState, goalState);
            if (plan != null)
            {
                currentActions = plan;
                worldDataProvider.PlanFound(goalState, plan);

                fsm.PopState();
                fsm.PushState(performActionState);

            }
            else
            {
                
                Debug.Log("Planning failed! Goal: ");
                Debug.Log(goalState.Keys.First());

                worldDataProvider.PlanFailed(goalState);
                fsm.PopState();
                fsm.PushState(planningState);
            }
        };
    }

    private void CreatePerformActionState()
    {
        performActionState = (fsm, gagent) =>
        {
            if (!HasActionPlan())
            {
                Debug.Log("Done with actions, planning new actions.");
                fsm.PopState();
                fsm.PushState(planningState);
                worldDataProvider.ActionsFinished();
                return;
            }

            GAction action = currentActions.Peek();
            if (action.IsDone())
            {
                currentActions.Dequeue();
            }

            if (HasActionPlan())
            {
                action = currentActions.Peek();
                bool success = action.Perform(gagent);

                if (!success)
                {
                    fsm.PopState();
                    fsm.PushState(planningState);
                    worldDataProvider.PlanAborted(action);
                }
            }
            else
            {
                fsm.PopState();
                if(RepeatAfterFinish) fsm.PushState(planningState);
                worldDataProvider.ActionsFinished();
            }
        };
    }

    private void FindDataProvider()
    {
        worldDataProvider = gameObject.GetComponent<IGoap>();
        if (worldDataProvider == null) worldDataProvider = gameObject.AddComponent<Gatherer>();
        //
    }

    private void FindBlackBoard()
    {
        blackBoard = GameObject.FindGameObjectWithTag("BlackBoard").GetComponent<GBlackBoard>();
        if (blackBoard == null) Debug.Log("No BlackBoard!");
    }

    public void TargetBlackBoard()
    {
        Target = blackBoard.gameObject;
    }

    public void LoadActions()
    {
        GAction[] actions = gameObject.GetComponents<GAction>();
        
        if(actions.Length == 0)
        {
            AddBasicActions();
        }

        foreach (GAction a in actions)
        {
            availableActions.Add(a);
            //Debug.Log("Added action: " + a.GetType());
        }
        
    }

    public void BuildCampfire()
    {
        Vector3 campfireLocation = new Vector3(transform.position.x, 0, transform.position.z + 3);
        GameObject instance = Instantiate(campfire, campfireLocation, Quaternion.identity);
        stats.campfire = instance.transform;
    }

    public void BuildBed()
    {
        Vector3 bedLocation = new Vector3(transform.position.x, 0, transform.position.z);
        GameObject instance = Instantiate(bed, bedLocation, Quaternion.identity);
        stats.bed = instance.transform;
    }

    public void AddBasicActions()
    {

    }

    public void MoveTowards(Vector3 destination)
    {
        Vector3 direction = destination - gameObject.transform.position;

        if (Vector3.Distance(destination, gameObject.transform.position) < moveSpeed) gameObject.transform.position = destination;
        else gameObject.transform.position += direction.normalized * moveSpeed;

        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z);
    }

}

