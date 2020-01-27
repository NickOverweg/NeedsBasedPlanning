using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWaterAction : MovementBaseAction
{
    public MoveToWaterAction()
    {
        AddEffect(nameof(StateKeys.Location), nameof(LocationType.Drinkable));

        locationType = LocationType.Drinkable;
    }
}
