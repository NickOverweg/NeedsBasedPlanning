using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToFoodAction : MovementBaseAction
{
    public MoveToFoodAction()
    {

        AddEffect(nameof(StateKeys.Location), nameof(LocationType.FoodGatherable));

        locationType = LocationType.FoodGatherable;
    }
}
