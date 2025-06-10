using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] AttackEffectData givenAttackEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl.pc.EffectsHolder.AddEffect(givenAttackEffect);
        Destroy(gameObject);
    }

}
