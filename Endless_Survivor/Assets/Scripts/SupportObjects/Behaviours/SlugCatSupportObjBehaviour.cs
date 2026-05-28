using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugCatSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] int _foodNeededToHibernate;
    [SerializeField] RandomBetweenTwoConstants _foodFoundOnDig;
    [SerializeField] RandomBetweenTwoConstants _digCooldown;
    [SerializeField] float _hpRegenPerSecondOnHibernation;
    [SerializeField] CustomAnimation _diggingAnimation;
    [SerializeField] CustomAnimation _hibernationStartAnimation;
    [SerializeField] CustomAnimation _hibernationAnimation;
    [SerializeField] ParticleSystem _diggingParticles;
    [SerializeField] int _diggingParticlesRenderingOffset;
    [SerializeField] ParticleSystem _dugFoodParticles;
    [SerializeField] int _dugFoodParticlesRenderingOffset;


    int _gatheredFood;
    float _digTimer;
    Vector2 _hibernatingPosition;
    bool _hibernating = false;
    bool _digging = false;
    float _hpRegenTimer;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var slugCatOriginal = original as SlugCatSupportObjBehaviour;
        _foodNeededToHibernate = slugCatOriginal._foodNeededToHibernate;
        _foodFoundOnDig = slugCatOriginal._foodFoundOnDig;
        _digCooldown = slugCatOriginal._digCooldown;
        _hpRegenPerSecondOnHibernation = slugCatOriginal._hpRegenPerSecondOnHibernation;
        _diggingAnimation = new(ObjControl.Animator, slugCatOriginal._diggingAnimation);
        _hibernationStartAnimation = new(ObjControl.Animator, slugCatOriginal._hibernationStartAnimation);
        _hibernationAnimation = new(ObjControl.Animator, slugCatOriginal._hibernationAnimation);

        ObjControl.Animator.AddAnimations(new() { _diggingAnimation, _hibernationStartAnimation, _hibernationAnimation });


        _diggingParticles = slugCatOriginal._diggingParticles;
        _diggingParticlesRenderingOffset = slugCatOriginal._diggingParticlesRenderingOffset;
        _dugFoodParticles = slugCatOriginal._dugFoodParticles;
        _dugFoodParticlesRenderingOffset = slugCatOriginal._dugFoodParticlesRenderingOffset;

        _digTimer = _digCooldown.rand;

        OnUpdate += DigCooldown;
        OnUpdate += Hibernate;

        PlayerControl.pc.PlayerHPManager.OnDamageTaken += StartHibernating;
    }
    void DigCooldown()
    {
        if (_hibernating || _digging)
            return;
        if (_digTimer > 0)
        {
            _digTimer -= Time.deltaTime;
                return;
        }
        if (ObjControl.Animator.CurrAnim.AnimationName != _diggingAnimation.AnimationName)
        {
            ObjControl.Animator.ChangeAnim(_diggingAnimation.AnimationName);
            return;
        }
        _digging = true;
        ParticleConfig digParticlesConfig = new(_diggingParticles, ObjControl.transform.position, Quaternion.identity, _diggingAnimation.AnimDuration);
        var digParticles = ParticleManager.pm.SpawnParticles(digParticlesConfig);
        digParticles.GetComponent<ParticleSortingOrderByY>().Offset = _diggingParticlesRenderingOffset;

        GameManager.gm.DelayAction(_diggingAnimation.AnimDuration, () => _digging = false, null);
        GameManager.gm.DelayAction(_diggingAnimation.AnimDuration, Dig, () => StoppedDigging(digParticles));
    }
    bool StoppedDigging(ParticleSystem diggingParticles)
    {
        if ((diggingParticles == null))
            return false;
        bool stoppedDigging = ObjControl.Animator.CurrAnim.AnimationName != _diggingAnimation.AnimationName;
        if(stoppedDigging)
        {
            GameObject.Destroy(diggingParticles);
            Dig();
        }
        return stoppedDigging;
    }
    void Dig()
    {
        _digTimer = _digCooldown.rand;
        //maybe add a bonus depending on the biome
        int dugFood = (int)_foodFoundOnDig.rand;
        _gatheredFood += dugFood;
        ParticleConfig dugFoodParticleConfig = new(_dugFoodParticles, ObjControl.transform.position, Quaternion.identity, _dugFoodParticles.main.duration);
        ParticleManager.pm.SpawnParticles(dugFoodParticleConfig).GetComponent<ParticleSortingOrderByY>().Offset = _dugFoodParticlesRenderingOffset;
        ObjControl.Animator.EndAnimation(_diggingAnimation.AnimationName);
    }
    void StartHibernating(int _)
    {
        if (_gatheredFood < _foodNeededToHibernate)
            return;

        ObjControl.Animator.ChangeAnim(_hibernationStartAnimation.AnimationName);
        
        GameManager.gm.DelayAction(_hibernationStartAnimation.AnimDuration, SetHibernatingValues, null);
    }
    void SetHibernatingValues()
    {
        _hibernating = true;
        _hibernatingPosition = ObjControl.transform.position;
        _hpRegenTimer = 1 / _hpRegenPerSecondOnHibernation;
        ObjControl.Animator.EndAnimation(_hibernationStartAnimation.AnimationName);
    }
    void Hibernate()
    {
        if (!_hibernating)
            return;
        if(ObjControl.Animator.CurrAnim.AnimationName != _hibernationAnimation.AnimationName)
        {
            ObjControl.Animator.ChangeAnim(_hibernationAnimation.AnimationName);
            return;
        }
        ObjControl.transform.position = _hibernatingPosition;
        _hpRegenTimer -= Time.deltaTime;
        if (_hpRegenTimer > 0)
            return;
        _hpRegenTimer = 1 / _hpRegenPerSecondOnHibernation;
        PlayerControl.pc.PlayerHPManager.Heal(1);
        _gatheredFood--;
        if( _gatheredFood == 0 || PlayerControl.pc.PlayerHPManager.RemainingHP == PlayerControl.pc.PlayerHPManager.MaxHP)
        {
            StopHibernating();
        }


    }
    void StopHibernating()
    {
        _digTimer = _digCooldown.rand;
        _hibernating = false;
        ObjControl.Animator.EndAnimation(_hibernationAnimation.AnimationName);
    }
}
