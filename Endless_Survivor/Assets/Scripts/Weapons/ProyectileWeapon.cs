using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileWeapon : ShootingWeapon
{
    ProyectileData _proyectileData;
    float _proyectileSpeed;
    float _proyectileSpread = 5f;
    public ProyectileData ProyectileData {  get { return _proyectileData; } set { _proyectileData = value; } }
    public float ProyectileSpread { get { return _proyectileSpread; } set { _proyectileSpread = value; } }
    public float ProyectileSpeed { get { return _proyectileSpeed; } set { _proyectileSpeed = value; } }
    public override void Attack()
    {
        base.Attack();
        WeaponControl.WeaponAnimator.ChangeAnim("Attack");
        Proyectile proyectile = Instantiate(GameManager.gm.prefabHolder.Prefabs["Proyectile"]).GetComponent<Proyectile>();
        proyectile.transform.position = FirePoint.position;
        proyectile.transform.rotation = FirePoint.rotation;
        proyectile.transform.Rotate(new Vector3(0, 0, Random.Range(-_proyectileSpread, _proyectileSpread)));
        proyectile.Speed = _proyectileSpeed;
        proyectile.LifeTime = WeaponStats.Range + PlayerControl.PlayerStats.Range;
        proyectile.Damage = WeaponStats.Damage + PlayerControl.PlayerStats.Damage;
        proyectile.SpriteRenderer.sprite = _proyectileData.ProyectileSprite;
        proyectile.Collider.size = _proyectileData.ColliderSize;
        proyectile.StartMoving();
    }
}
