using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayWeapon : ShootingWeapon
{
    LineRenderer _lineRenderer;

    public LineRenderer LineRenderer {  get { return _lineRenderer; } set { _lineRenderer = value; } }
    public override void Attack()
    {
        base.Attack();

        WeaponAnimator.ChangeAnim("Shoot");
        RaycastHit2D hit = Physics2D.Raycast(base.FirePoint.position, FirePoint.right);
        if (!hit)
        {
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, FirePoint.position + FirePoint.right * 100);//if the player didn't hit nothing (which should not happen), setting the end of the ray far enough so the player can't see it
            return;
            
        }
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);
        EnemyHP enemyHP = hit.collider.GetComponent<EnemyHP>();
        if (enemyHP != null)
            enemyHP.RecieveDamage(WeaponStats.Damage + GameManager.gm.selectedCharacter.PlayerStats.Damage);
    }

    public void UpdateLinePosition()
    {
        _lineRenderer.SetPosition(0, FirePoint.position);
    }

    public void ShowShootLine()
    {
        _lineRenderer.gameObject.SetActive(true);
    }

    public void HideShootLine()
    {
        _lineRenderer.gameObject.SetActive(false);
    }
}
