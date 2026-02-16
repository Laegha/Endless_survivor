using UnityEngine;

public class ActivateWhenHighSpeedItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _behavioursToTrigger;
    [SerializeField] float _speedThreshold;
    [SerializeField] float _speedTop;
    float _playerSpeed => PlayerControl.pc.PlayerRb.velocity.magnitude;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var activateWhenHighSpeedOriginal = original as ActivateWhenHighSpeedItemBehaviour;
        _behavioursToTrigger = activateWhenHighSpeedOriginal._behavioursToTrigger;
        _speedThreshold = activateWhenHighSpeedOriginal._speedThreshold;
        _speedTop = activateWhenHighSpeedOriginal._speedTop;
        behaviourManager.onUpdate += CheckSpeed;
    }
    void CheckSpeed()
    {
        if (_playerSpeed < _speedThreshold || _playerSpeed > _speedTop)
            return;

        foreach (var behaviourId in _behavioursToTrigger)
        {
            var behaviour = BehaviourManager.ItemBehaviours.Find(x => x.BehaviourId == behaviourId);
            behaviour.Activate();
        }

    }
}
