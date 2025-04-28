using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    int _damage;
    List<Collider2D> _ignoreColliders;
    public int Damage { get { return _damage; } set {  _damage = value; } }
    public List<Collider2D> IgnoreColliders { get { return _ignoreColliders; } set { _ignoreColliders = value; } }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(_ignoreColliders.Contains(other)) 
            return;
        HP hitHP = other.GetComponent<HP>();
        if(hitHP == null )
            return;
        hitHP.TakeDamage(Damage);
    }
}
