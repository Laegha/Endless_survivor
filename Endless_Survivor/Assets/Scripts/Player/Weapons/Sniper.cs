using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : Weapon
{
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _firePoint;
    [SerializeField] Animator _animator;

    public override void Attack()
    {
        base.Attack();

        _animator.Play("Shoot");
        RaycastHit2D hit = Physics2D.Raycast(_firePoint.position, _firePoint.right);
        if (!hit)
        {
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _firePoint.position + _firePoint.right * 100);//if the player didn't hit nothing (which should not happen), setting the end of the ray far enough so the player can't see it
            return;
            
        }
        hit.collider.GetComponent<EnemyHP>().RecieveDamage(Data.WeaponStats.Damage + GameManager.gm.selectedCharacter.PlayerStats.Damage);

    }

    public void UpdateLinePosition()
    {
        _lineRenderer.SetPosition(0, _firePoint.position);
    }
}
