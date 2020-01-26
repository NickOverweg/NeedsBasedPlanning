using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GAgent : MonoBehaviour
{
    private GFSM stateMachine;

    private GFSM.IGFSMState planningState; 
    private GFSM.IGFSMState performActionState;

    public GameObject Target;
    
    [SerializeField]
    public float moveSpeed = 1;

    public AgentInventory inventory;

    private HashSet<GAction> availableActions;
    private Queue<GAction> currentActions;

    private IGoap worldDataProvider;
    private GPlanner planner;
    private GBlackBoard blackBoard;

    public GBlackBoard BlackBoard
    {
        get { return blackBoard; }
    }

    private MaslowLevel maslowLevel = MaslowLevel.none;

    public MaslowLevel MasLevel
    {
        get { return maslowLevel; }
        set { maslowLevel = value; }
    }

    public bool NearTarget
    {
        get { return nearTarget; }
        set { nearTarget = value; }
    }

    private bool nearTarget = false;

    private void Start()
    {
        stateMachine = new GFSM();
        availableActions = new HashSet<GAction>();
        currentActions = new Queue<GAction>();
        planner = new GPlanner();


        //availableActions.
        //gameObject.GetComponents<GAction>()

        inventory = GetComponent<AgentInventory>();

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
                fsm.PushState(planningState);
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

    public void AddBasicActions()
    {
        gameObject.AddComponent<GoToBlackBoardAction>();
        gameObject.AddComponent<MoveToAction>();
        gameObject.AddComponent<GetJobAction>();
    }

    public void MoveTowards(Transform destination)
    {
        Vector3 direction = destination.position - gameObject.transform.position;

        gameObject.transform.position += direction.normalized * moveSpeed;
    }

}

