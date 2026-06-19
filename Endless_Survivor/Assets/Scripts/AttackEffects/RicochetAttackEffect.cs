using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RicochetAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _ricochetAmmount;//for now it's only one ricochet, until i figure out a way to stack different effects values into one single pool
    static List<RicochetChain> _activeRicochets = new();
    RicochetChain _myChain;
    public RicochetAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var ricochetOriginal = original as RicochetAttackEffect;
        _ricochetAmmount = ricochetOriginal._ricochetAmmount;


        OnAttack += GetChain;
        OnEnemyHit += GenerateRicochetAttack;
    }

    void GetChain()
    {
        GameManager.gm.DelayActionAFrame(() =>
        {
            _myChain = _activeRicochets.Find(chain => chain.currAttack == AffectedAttack);
            if (_myChain == null)
            {
                _myChain = new RicochetChain(AffectedAttack, _ricochetAmmount);
                _activeRicochets.Add(_myChain);
            }

        }, null);
    }
    void GenerateRicochetAttack(EnemyControl hitEnemy)
    {
        if (_myChain == null)
            return; 
        if (_myChain.ricochetsLeft <= 0)
        {
            _activeRicochets.Remove(_myChain);
            return;
        }
        _myChain.ricochetsLeft--;
        Vector2 attackDir = hitEnemy.transform.position - AffectedAttack.transform.position;
        Vector2 hitPoint = Physics2D.Raycast(AffectedAttack.transform.position, attackDir, Mathf.Infinity, Utility.GetCollidableLayers("PlayerAttack")).point;
        
        List<GameObject> enemies = EnemySpawnManager.esm.Enemies;
        if (enemies.Count < 2)//if there are no enemies to ricochet to, gg
            return;
        GameObject ricochetTargetedEnemy = Utility.GetClosestTo(enemies, hitEnemy.transform)[1];
        Vector2 ricochetAttackDir = (Vector2)ricochetTargetedEnemy.transform.position - hitPoint;
        List<Collider2D> ignoreColliders = hitEnemy.transform.root.GetComponentsInChildren<Collider2D>().ToList();
        Attack nextAttackInChain;
        AffectedAttack.ParentWeapon.Attack(hitPoint, ricochetAttackDir.normalized, true, out nextAttackInChain, ignoreColliders);
        _myChain.currAttack = nextAttackInChain;
    }
}

class RicochetChain
{
    public Attack currAttack;
    public int ricochetsLeft;

    public RicochetChain(Attack currAttack, int ricochetsLeft)
    {
        this.currAttack = currAttack;
        this.ricochetsLeft = ricochetsLeft;
    }
}