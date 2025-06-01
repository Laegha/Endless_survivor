using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackEffectSinergy
{
    public void Initiate(Attack attack, AttackEffect effect)
    {

    }
}

public class AttackWithEffectSinergy<A, E> : IAttackEffectSinergy where A : Attack where E : AttackEffect
{
    public A attack;
    public E effect;

    public void Initiate(Attack attack, AttackEffect effect)
    {
        this.attack = (A)attack;
        this.effect = (E)effect;

        AssignActions();
    }

    public virtual void AssignActions() { }
}