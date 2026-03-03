using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeAnimBasedOnHPSupportObjBehaviour : UseHPSupportObjBehaviour
{
    new public static int maxStacks => 1;
    [Tooltip("Nums are thresholds that trigger when hp is below")][SerializeField] List<GenericNumHolder<CustomAnimation>> _animationsByHP;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var changeAnimOriginal = original as ChangeAnimBasedOnHPSupportObjBehaviour;
        _animationsByHP = new();
        //copy animations as new
        foreach(var animation in changeAnimOriginal._animationsByHP)
        {
            GenericNumHolder<CustomAnimation> animCopy = new GenericNumHolder<CustomAnimation>();
            animCopy.generic = new(ObjControl.Animator, animation.generic);
            animCopy.num = animation.num;
            _animationsByHP.Add(animCopy);
        }
        _animationsByHP.Sort((anim1, anim2) => anim1.num.CompareTo(anim2.num));
        
        //add animations to animator
        List<CustomAnimation> anims = _animationsByHP.Select(x => x.generic).ToList();
        ObjControl.Animator.AddAnimations(anims);


        OnDamage += CheckCurrAnim;
        OnHeal += CheckCurrAnim;
    }

    void CheckCurrAnim()
    {
        int currHP = HpBehaviour.SupportObjHP.RemainingHP;
        for(int i = 0; i < _animationsByHP.Count; i++)
        {
            if(currHP < _animationsByHP[i].num)
            {
                ObjControl.Animator.ChangeAnim(_animationsByHP[i].generic.AnimationName);
                break;
            }
        }
    }
}
