using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherWoodActionDEPR : GAction
{
    private bool woodGathered = false;

    private float startTime = 0f;
    public float GatherDuration = 2;

    public GatherWoodActionDEPR()
    {
        AddPrecondition("hasWood", false);
        AddPrecondition("nearTarget", false);
        AddEffect("hasWood", true);
    }

    public override void ResetVariables()
    {
        woodGathered = false;
        target = null;
        startTime = 0;
    }

    public override bool IsDone()
    {
        return woodGathered;
    }
    
    public override bool CheckProceduralPrecondition(GAgent agent)
    {
        //set target if none is set yet. 
        if (target == null) target = agent.BlackBoard.RequestSourceLocation(SourceNames.Forest);

        //if target is still null, no target is known.
        if (target == null) return false;


        #region ironRockCode
            /*
            // find the nearest rock that we can mine
            IronRockComponent[] rocks = FindObjectsOfType(typeof(IronRockComponent)) as IronRockComponent[];
            IronRockComponent closest = null;
            float closestDist = 0;

            foreach (IronRockComponent rock in rocks)
            {
                if (closest == null)
                {
                    // first one, so choose it for now
                    closest = rock;
                    closestDist = (rock.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    // is this one closer than the last?
                    float dist = (rock.gameObject.transform.position - agent.transform.position).magnitude;
                    if (dist < closestDist)
                    {
                        // we found a closer one, use it
                        closest = rock;
                        closestDist = dist;
                    }
                }
            }
            targetRock = closest;
            target = targetRock.gameObject;

            return closest != null;
            */
            #endregion
        return true;
    }

    public override bool Perform(GAgent agent)
    {
        if (startTime == 0) startTime = Time.time;

        if (Time.time - startTime > GatherDuration)
        { //finished
            Debug.Log("wood gathered");
        }

        return true;
    }
}