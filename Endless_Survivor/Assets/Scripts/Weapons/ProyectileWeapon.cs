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

    public override void Start()
    {
        base.Start();
        InitializeAttack += InitiateProyectile;
    }
    public override void Attack()
    {
        base.Attack();
        WeaponControl.WeaponAnimator.ChangeAnim("Attack");
        Proyectile proyectile = Instantiate(GameManager.gm.prefabHolder.Prefabs["Proyectile"]).GetComponent<Proyectile>();
        InitializeAttack.Invoke(proyectile);

    }
    void InitiateProyectile(Attack affectedAttack)
    {
        var proyectile = affectedAttack as Proyectile;
        proyectile.transform.position = FirePoint.position;
        proyectile.transform.rotation = FirePoint.rotation;

        float proyectileSpeed = _proyectileSpeed;
        float proyectileLifeTime = WeaponStats.Range + PlayerControl.PlayerStats.Range;
        int proyectileDamage = WeaponStats.Damage + PlayerControl.PlayerStats.Damage;
        proyectile.Initiate(proyectileDamage, proyectileSpeed, proyectileLifeTime, _proyectileData, _proyectileSpread);
    }
}
