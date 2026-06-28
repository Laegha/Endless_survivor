using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class ReviveEnemiesInAreaSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] float _revivedHPPercent;
    [SerializeField] Vector2 _spawnOffset;
    [SerializeField] Material _revivedMaterial;
    [SerializeField] CustomAnimation _revivingAnimation;
    [SerializeField] int _reviveFrame;
    [SerializeField] bool _destroyAfterRevive;
    Vector2 _spawningTilePos => (Vector2)ObjControl.transform.position + _spawnOffset;

    List<EnemyControl> _possibleRevivingEnemies = new();
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var reviveEnemiesOriginal = original as ReviveEnemiesInAreaSupportObjBehaviour;
        _revivedHPPercent = reviveEnemiesOriginal._revivedHPPercent;
        _spawnOffset = reviveEnemiesOriginal._spawnOffset;
        _revivedMaterial = reviveEnemiesOriginal._revivedMaterial;
        _reviveFrame = reviveEnemiesOriginal._reviveFrame;
        _destroyAfterRevive = reviveEnemiesOriginal._destroyAfterRevive;

        _revivingAnimation = new(ObjControl.Animator, reviveEnemiesOriginal._revivingAnimation);
        ObjControl.Animator.AddAnimations(new() { _revivingAnimation });

        OnObjEnterArea += AddReviveEnemy;
        OnObjExitArea += RemoveReviveEnemy;
        OnDestroyed += CleanEnemiesActions;
    }
    void CheckForDuplicates()
    {
        Dictionary<EnemyControl, int> duplicates = new Dictionary<EnemyControl, int>();
        foreach(var enemy in _possibleRevivingEnemies)
        {
            int duplicateCount = _possibleRevivingEnemies.Where(x => x == enemy).Count();
            if (duplicateCount > 1)
                duplicates.Add(enemy, duplicateCount);
        }
        foreach(var duplicateEnemy in duplicates)
        {
            for(int i = 0; i < duplicateEnemy.Value; i++)
            {
                duplicateEnemy.Key.EnemyHP.OnDeath -= ReviveEnemy;
            }
        }
    }
    void AddReviveEnemy(GameObject obj)
    {
        EnemyControl enemyControl = Utility.FindFirstComponentInParent<EnemyControl>(obj);
        if (enemyControl == null || _possibleRevivingEnemies.Contains(enemyControl))
            return;
        _possibleRevivingEnemies.Add(enemyControl);
        enemyControl.EnemyHP.OnDeath += ReviveEnemy;
    }
    void RemoveReviveEnemy(GameObject obj)
    {
        EnemyControl enemyControl = Utility.FindFirstComponentInParent<EnemyControl>(obj);
        if (enemyControl == null)
            return;
        _possibleRevivingEnemies.Remove(enemyControl);
        enemyControl.EnemyHP.OnDeath -= ReviveEnemy;
    }
    void ReviveEnemy(EnemyControl deadEnemy)
    {
        ObjControl.Animator.ChangeAnim(_revivingAnimation.AnimationName);
        float frameDelay = _reviveFrame / _revivingAnimation.FramesPerSecond;
        GameManager.gm.DelayAction(frameDelay,() =>
        {
            var revivingEnemyData = deadEnemy.EnemyData;
            Vector2 spawningPos = Utility.IsTileUsable(_spawningTilePos) ? _spawningTilePos : ObjControl.transform.position;
            Tile spawningTile = MapManager.mm.GenerationHandler.TileMatrix[spawningPos][0];
            
            GameObject enemy = EnemySpawnManager.esm.SpawnEnemy(spawningTile, revivingEnemyData);
            EnemyControl enemyControl = enemy.GetComponent<EnemyControl>();
            enemyControl.MaterialManager.SetMaterialOverride(new(10, _revivedMaterial));

            enemyControl.EnemyHP.InitializeHP((int)(enemyControl.EnemyHP.MaxHP * _revivedHPPercent / 100));
            if(_destroyAfterRevive)
                DestroyObj();
        } , () => ObjControl == null);
        
    }

    void CleanEnemiesActions()
    {
        foreach(var enemy in _possibleRevivingEnemies)
        {
            enemy.EnemyHP.OnDeath -= ReviveEnemy;
        }
    }
}
