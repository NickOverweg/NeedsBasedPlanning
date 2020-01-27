using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToBedAction : MovementBaseAction
{
    public MoveToBedAction()
    {

        AddEffect(nameof(StateKeys.Location), nameof(LocationType.Bed));

        locationType = LocationType.Bed;
    }
}
