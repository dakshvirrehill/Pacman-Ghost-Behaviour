using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToHouseStateBehaviour : GhostBehaviour
{

    override public void OnStateEnter(Animator pFSM, AnimatorStateInfo pStateInfo, int pLayerIndex)
    {
        SetupComponentReferences(pFSM);
    }

    //override public void OnStateUpdate(Animator pFSM, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    //override public void OnStateExit(Animator pFSM, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

}
