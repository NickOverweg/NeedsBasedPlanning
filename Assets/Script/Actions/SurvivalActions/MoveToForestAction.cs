using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToForestAction : MovementBaseAction
{
    public MoveToForestAction()
    {

        AddEffect(nameof(StateKeys.Location), nameof(LocationType.Forest));

        locationType = LocationType.FoodGatherable;
    }
}
