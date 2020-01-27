using System;
using System.Collections.Generic;
using UnityEngine;

public class GFSM
{
    private Stack<IGFSMState> stateStack = new Stack<IGFSMState>();

    public delegate void IGFSMState(GFSM fsm, GAgent agent);

    public void Update(GAgent agent)
    {

        
        if(stateStack.Count > 0 && stateStack.Peek() != null)
        {
            stateStack.Peek().Invoke(this, agent);
        }
    }

    public void PushState(IGFSMState state)
    {
        stateStack.Push(state);
    }

    public void PopState()
    {
        stateStack.Pop();
    }

}

