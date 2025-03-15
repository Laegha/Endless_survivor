using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageSource : MonoBehaviour
{
    int _damage;
    public int Damage { get { return _damage; } set { _damage = value; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerControl playerControl = collision.GetComponent<PlayerControl>();
        if (playerControl == null)
            return;
        playerControl.PlayerHPManager.TakeDamage(_damage);
    }
}
