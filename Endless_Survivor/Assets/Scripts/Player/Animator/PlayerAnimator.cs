using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAnimator : CustomAnimator
{
    CharacterData _characterData;
    string _currAnimationName;
    Vector2 _playerMovement;

    public override void ChangeAnim(string animName)
    {
        //_currAnim
        var possibleAnimations = _characterData.Animations.Where(anim => anim.AnimationName == animName).ToList();
        CoordinateAnimation closestAnim = possibleAnimations[0];
        foreach (var possibleAnim in possibleAnimations)
        {
            float closestDist = (_playerMovement - closestAnim.Coordinates).magnitude;
            float currAnimDist = (_playerMovement - possibleAnim.Coordinates).magnitude;
            if (currAnimDist < closestDist)
                closestAnim = possibleAnim;
        }
        CurrAnim = closestAnim;
        base.ChangeAnim(animName);
    }

    public void SetMovement(Vector2 playerMovement)
    {
        if(playerMovement != _playerMovement)
        {
            _playerMovement = playerMovement;
            ChangeAnim(CurrAnim.AnimationName);
        }
    }
}
