using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class InkyChaseStateBehaviour : ChaseStateBehaviour
{
    public string mBlinkyObjectName;
    public Transform mBlinkyTransform;

    public override void OnStateEnter(Animator pFSM, AnimatorStateInfo pStateInfo, int pLayerIndex)
    {
        if(mBlinkyTransform == null)
        {
            foreach(GhostController aGhost in GameDirector.Instance.Ghosts)
            {
                if(aGhost.gameObject.name == mBlinkyObjectName)
                {
                    mBlinkyTransform = aGhost.gameObject.transform;
                    break;
                }
            }
        }
        base.OnStateEnter(pFSM, pStateInfo, pLayerIndex);
    }


    protected override void UpdatePath()
    {
        Vector2 aBlinkyTransform = new Vector2(mBlinkyTransform.position.x, mBlinkyTransform.position.y);
        Vector2 aPacmanTransform = new Vector2(mController.PacMan.transform.position.x, mController.PacMan.transform.position.y);
        Vector2 aDesiredLocation = aBlinkyTransform + ((aPacmanTransform + mController.PacMan.MoveDirections[(int)mController.PacMan.moveDirection] * 2) - aBlinkyTransform) * 2;
        if(aDesiredLocation != mController.moveToLocation)
        {
            mController.moveToLocation = aDesiredLocation;
            mController.moveComplete();
        }
        mCurrentTimer = 0.0f;
    }
}

