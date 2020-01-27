using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentInventory : MonoBehaviour
{
    public int numWood;
    public int numLeaf;
    public int numFood;

    public int gathered;

    private void Start()
    {
        numFood = 0;
        numLeaf = 0;
        numWood = 0;
    }
}
