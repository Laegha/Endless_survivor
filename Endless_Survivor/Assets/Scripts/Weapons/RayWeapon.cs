using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayWeapon : ShootingWeapon
{
    LineRenderer _lineRenderer;
    bool _shooting = false;
    public LineRenderer LineRenderer {  get { return _lineRenderer; } set { _lineRenderer = value; } }

    public override void Start()
    {
        base.Start();
        WeaponControl.WeaponAnimator.Animations.Where(anim => anim.AnimationName == "Attack").ToArray()[0].OnAnimationEnd += StopShooting;
    }

    public override void Update()
    {
        base.Update();
        if(_shooting)
            UpdateLinePosition();
    }
    public override void Attack()
    {
        base.Attack();

        WeaponControl.WeaponAnimator.ChangeAnim("Attack");
        StartShooting();
        RaycastHit2D hit = Physics2D.Raycast(FirePoint.position, -FirePoint.right);
        if (!hit)
        {
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, FirePoint.position + -FirePoint.right * 100);//if the player didn't hit nothing (which should not happen), setting the end of the ray far enough so the player can't see it
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

    public void StartShooting()
    {
        _shooting = true;
        _lineRenderer.gameObject.SetActive(true);
    }

    public void StopShooting(CustomAnimator placeholder)
    {
        _shooting = false;
        print("Stop shooting");
        _lineRenderer.gameObject.SetActive(false);
    }
}
