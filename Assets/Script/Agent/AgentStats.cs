using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentStats : MonoBehaviour
{
    public int hunger;
    public int thirst;
    public Transform campfire;
    public Transform bed;

    public void Start()
    {
        hunger = 100;
        thirst = 100;
        campfire = null;
        bed = null;
    }

}
