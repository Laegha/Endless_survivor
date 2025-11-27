using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class StealPickupsSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] float _moveSpeed = 2.5f;
    [SerializeField] float _triggerStealDistance = .5f;
    [SerializeField] CustomAnimation _startAnimation;
    [SerializeField] CustomAnimation _movingAnimation;
    [SerializeField] CustomAnimation _stealAnimation;
    [SerializeField] int _destroyPickupFrame = 0;
    [SerializeField] CustomAnimation _fleeAnimation;
    [SerializeField] CustomAnimation _failedTheftAnimation;

    PickupControl _stealingPickup;
    Vector2 _moveDirection;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        StealPickupsSupportObjBehaviour stealPickupsOriginal = original as StealPickupsSupportObjBehaviour;
        _moveSpeed = stealPickupsOriginal._moveSpeed;
        _triggerStealDistance = stealPickupsOriginal._triggerStealDistance;
        _startAnimation = new CustomAnimation(control.Animator, stealPickupsOriginal._startAnimation);
        _movingAnimation = new CustomAnimation(control.Animator, stealPickupsOriginal._movingAnimation);
        _stealAnimation = new CustomAnimation(control.Animator, stealPickupsOriginal._stealAnimation);
        _destroyPickupFrame = stealPickupsOriginal._destroyPickupFrame;
        _fleeAnimation = new CustomAnimation(control.Animator, stealPickupsOriginal._fleeAnimation);
        _failedTheftAnimation = new CustomAnimation(control.Animator, stealPickupsOriginal._failedTheftAnimation);

        _startAnimation.Events.Add(new(null, _startAnimation.Frames.Length - 1,() =>{ control.Animator.EndAnimation(_startAnimation); StartMovingToPickup(); }));
        _stealAnimation.Events.Add(new(null, _stealAnimation.Frames.Length - 1, Flee));
        _stealAnimation.Events.Add(new(null, _destroyPickupFrame, () => GameObject.Destroy(_stealingPickup.gameObject)));
        _failedTheftAnimation.Events.Add(new(null, _failedTheftAnimation.Frames.Length - 1, Flee));
        _fleeAnimation.Events.Add(new(null, _fleeAnimation.Frames.Length - 1, () => GameObject.Destroy(control.gameObject)));

        control.Animator.AddAnimations(new() { _startAnimation, _movingAnimation, _stealAnimation, _fleeAnimation, _failedTheftAnimation });

        OnStart += SearchPickup;
        OnStart += () => control.Animator.ChangeAnim(_startAnimation);

    }
    void SearchPickup()
    {
        var pickupsOnGround = GameObject.FindObjectsOfType<PickupControl>();
        _stealingPickup = pickupsOnGround[Random.Range(0, pickupsOnGround.Length)];//maybe this should be a roulette, whith better pickups having a higher chance of being stolen

    }
    void StartMovingToPickup()
    {
        if (_stealingPickup == null)
        {
            ObjControl.Animator.ChangeAnim(_failedTheftAnimation);
            return;
        }
        _moveDirection = (_stealingPickup.transform.position - ObjControl.transform.position).normalized;
        if(_moveDirection.x < 0)
            ObjControl.Renderers.ForEach(renderer => renderer.flipX = true);
        ObjControl.Animator.ChangeAnim(_movingAnimation);
        OnUpdate += MoveToPickup;
    }
    void MoveToPickup()
    {
        if(_stealingPickup == null)
        {
            //Play an anger animation, then flee
            ObjControl.Animator.ChangeAnim(_failedTheftAnimation);
            OnUpdate -= MoveToPickup;
            return;
        }
        ObjControl.transform.Translate(_moveDirection * _moveSpeed * Time.deltaTime);
        float distance = Vector2.Distance(ObjControl.transform.position, _stealingPickup.transform.position);
        if(distance <= _triggerStealDistance)
        {
            //Destroy pickup colliders so the player can no longer pick it up
            var pickupColliders = _stealingPickup.GetComponentsInChildren<Collider2D>();
            foreach(var collider in pickupColliders)
            {
                GameObject.DestroyImmediate(collider);
            }
            ObjControl.Animator.ChangeAnim(_stealAnimation);
            OnUpdate -= MoveToPickup;
        }
    }
    void Flee()
    {
        ObjControl.Animator.ChangeAnim(_fleeAnimation);
    }
}
