using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/**
 * Plans what actions can be completed in order to fulfill a goal state.
 */
public class GPlanner
{
    //TODO: public int MaxDepth = 10;

    public Queue<GAction> Plan (GAgent agent, HashSet<GAction> availableActions, Dictionary<string, object> worldState, Dictionary<string, object> goalState)
    {
        foreach (GAction a in availableActions)
        {
            a.ResetVariables();
        }

        // check what actions can run using their checkProceduralPrecondition
        HashSet<GAction> usableActions = new HashSet<GAction>();
        foreach (GAction a in availableActions)
        {
            if (a.CheckProceduralPrecondition(agent))
                usableActions.Add(a);
        }

        List<GNode> leaves = new List<GNode>();
        Debug.Log("Started planing");

        GNode start = new GNode(null, 0, worldState, null);
        bool success = BuildGraph(start, leaves, usableActions, goalState);

        if (!success)
        {
            Debug.Log("No plan found!");
            return null;
        }

        GNode cheapest = null;
        foreach(GNode leaf in leaves)
        {
            if (cheapest == null) cheapest = leaf;
            else
            {
                if (leaf.runningCost < cheapest.runningCost) cheapest = leaf;
            }
        }

        List<GAction> result = new List<GAction>();
        GNode n = cheapest;

        while (n != null) //puts the cheapest branch of leaves in order in result
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        //is this necesary? maybe implement this loop in previous loop.
        Queue<GAction> queue = new Queue<GAction>();
        foreach (GAction a in result)
        {
            queue.Enqueue(a);
        }

        return queue;
    }

    private bool BuildGraph(GNode parent, List<GNode> leaves, HashSet<GAction> usableActions, Dictionary<string, object> goalState)
    {
        bool foundOne = false;

        // go through each action available at this node and see if we can use it at this point. 
        foreach(GAction action in usableActions)
        {
            if(IsInState(action.Preconditions, parent.state))
            {
                Dictionary<string, object> currentState = PopulateState(parent.state, action.Effects);
                GNode node = new GNode(parent, parent.runningCost + action.cost, currentState, action);

                if(IsInState(goalState, currentState))
                {
                    leaves.Add(node);
                    foundOne = true;
                } else
                {
                    HashSet<GAction> subset = ActionSubset(usableActions, action);

                    bool found = BuildGraph(node, leaves, subset, goalState);
                    if (found) foundOne = true;
                }
            }
        }
        return foundOne;
    }

    //Makes a new set of actions by removing one action from a set of actions.
    //SUBSET? WHY? This removes the possibility for repeating actions later in the tree. 
    //on the other hand this forces smaller goals to be set, which is probably for the best. 
    //should probably make an exception for movement?
    //
    private HashSet<GAction> ActionSubset(HashSet<GAction> actions, GAction removeMe)
    {
        HashSet<GAction> subset = new HashSet<GAction>(actions);
        subset.Remove(removeMe);

        return subset;
    }

    //check if a state is in a goalstate.
    private bool IsInState(Dictionary<string, object> goalState, Dictionary<string, object> testState)
    {
        if (goalState.Count == 0) return true;

        //test for missing keys
        if (goalState.Keys.Except(testState.Keys).Count() > 0) return false;

        bool isInState = goalState.Keys.All(key => testState.ContainsKey(key) && testState[key].Equals(goalState[key]));

        return isInState;

        //Dictionary<string, object> differences = testState.Where(entry => goalState.ElementAtOrDefault entry.Key != entry.Value)
        //   .ToDictionary(entry => entry.Key, entry => entry.Value);
        

        //if (differences.Count > 0) return false;

        //return true;
    }

    private Dictionary<string, object> PopulateState(Dictionary<string, object> currentState, Dictionary<string, object> stateChanges)
    {
        Dictionary<string, object> state = new Dictionary<string, object>(currentState);
        
        //adds unknown keys to the dictionary and overwrites known ones.
        stateChanges.ToList().ForEach(entry => state[entry.Key] = entry.Value);

        return state;
    }
}
