using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PullEnemiesInAreaToSelfSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] float _pullStrength;
    [SerializeField] int _pullForcePriority;
    [SerializeField] CustomAnimation _pullAnim;
    [SerializeField] ParticleSystem _pullParticles;
    Dictionary<RbForcesController, Vector2> _pullingRbs = new();

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var pullEnemiesOriginal = original as PullEnemiesInAreaToSelfSupportObjBehaviour;
        _pullStrength = pullEnemiesOriginal._pullStrength;
        _pullForcePriority = pullEnemiesOriginal._pullForcePriority;
        _pullAnim = new(ObjControl.Animator, pullEnemiesOriginal._pullAnim);
        ObjControl.Animator.AddAnimations(new() { _pullAnim });
        _pullParticles = pullEnemiesOriginal._pullParticles;

        OnStart += StartGfx;
        OnObjUpdateArea += ApplyForceToRb;
        OnDestroyed += StopAllRbs;
    }
    void StartGfx()
    {
        ObjControl.Animator.ChangeAnim(_pullAnim.AnimationName);
        ParticleConfig particlesConfig = new(_pullParticles, Vector2.zero, Quaternion.identity, -1, ObjControl.transform, true, true);
        ParticleManager.pm.SpawnParticles(particlesConfig);
    }
    void ApplyForceToRb(GameObject obj)
    {
        var objRb = obj.GetComponent<RbForcesController>();
        if (objRb == null)
            return;
        Vector2 direction = ObjControl.transform.position - obj.transform.position;
        if (_pullingRbs.ContainsKey(objRb) && _pullingRbs[objRb] == direction)
            return;
        RbForceInfo rbForceInfo = new(direction, _pullStrength, _pullForcePriority, ForceMode2D.Force);
        objRb.ChangeCurrForce(rbForceInfo);
        if (!_pullingRbs.ContainsKey(objRb))
            _pullingRbs.Add(objRb, direction);
    }
    void StopAllRbs()
    {
        foreach (var objRb in _pullingRbs)
        {
            objRb.Key.Stop();
        }

    }
}
