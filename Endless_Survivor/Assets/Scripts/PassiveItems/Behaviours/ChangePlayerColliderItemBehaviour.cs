using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerColliderItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] CapsuleDirection2D _newColliderDirection;
    [SerializeField] Vector2 _newColliderSize;
    [SerializeField] Vector2 _newColliderOffset;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var changeColOriginal = original as ChangePlayerColliderItemBehaviour;
        _newColliderDirection = changeColOriginal._newColliderDirection;
        _newColliderSize = changeColOriginal._newColliderSize;
        _newColliderOffset = changeColOriginal._newColliderOffset;
    }
    public override void Activate()
    {
        base.Activate();
        PlayerControl.pc.ChangeCollider(_newColliderDirection, _newColliderSize, _newColliderOffset);
    }
    public override void RemoveBehaviour()
    {

    }
}
