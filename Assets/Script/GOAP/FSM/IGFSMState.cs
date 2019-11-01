using UnityEngine;
using System.Collections;

public interface IGFSMState
{
    void Update(GFSM fsm, GameObject gameObject);
}

