using System;
using System.Collections.Generic;
using UnityEngine;

public class GNode
{
    public GNode parent;
    public float runningCost;
    public Dictionary<string, object> state;
    public GAction action;

    public GNode(GNode parent, float runningCost, Dictionary<string, object> state, GAction action)
    {
        this.parent = parent;
        this.runningCost = runningCost;
        this.state = state;
        this.action = action;
    }
}
