using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAnimator : CustomAnimator
{
    string _currAnimationName;
    Vector2 _playerMovement;
    List<CoordinateAnimation> _coordinateAnimations = new List<CoordinateAnimation>();

    public override void ChangeAnim(string animName)
    {
        var possibleAnimations = _coordinateAnimations.Where(anim => anim.AnimationName == animName).ToList();
        CoordinateAnimation closestAnim = possibleAnimations[0];
        foreach (var possibleAnim in possibleAnimations)
        {
            float closestDist = (_playerMovement - closestAnim.Coordinates).magnitude;
            float currAnimDist = (_playerMovement - possibleAnim.Coordinates).magnitude;
            if (currAnimDist < closestDist)
                closestAnim = possibleAnim;
        }
        if (CurrAnim == closestAnim)
            return;
        CurrAnim = closestAnim;
        AnimTimer = 0;
    }

    public void SetMovement(Vector2 playerMovement)
    {
        if(playerMovement != _playerMovement)
        {
            _playerMovement = playerMovement;
            ChangeAnim(CurrAnim.AnimationName);
        }
    }
    public override void AddAnimations(List<CustomAnimation> newAnimations)
    {
        base.AddAnimations(newAnimations);
        newAnimations.ForEach(anim => _coordinateAnimations.Add(anim as CoordinateAnimation));
    }
}
