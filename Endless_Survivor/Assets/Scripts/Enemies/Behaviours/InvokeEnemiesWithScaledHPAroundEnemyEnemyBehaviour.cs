using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvokeEnemiesWithScaledHPAroundEnemyEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [Tooltip("Use this for enemies that are always invoked")][SerializeField] List<EnemyData>_fixedInvokedEnemies;
    [Tooltip("N of these enemies will be invoked, leave empty if none")][SerializeField] List<RouletteElementChance<EnemyData>>_randomInvokedEnemies;
    [Tooltip("The weighted ammounts if using random enemies, isn't necessary if all are fixed")][SerializeField] List<RouletteElementChance<int>> _randomInvokePossibleAmmounts;
    [SerializeField] CustomAnimation _invokingAnim;
    [SerializeField] int _invokeFrame;
    [SerializeField] float _hpMultiplier = 1.0f;

    [SerializeReference] IPattern _invokationPattern;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var invokeEnemiesOriginal = original as InvokeEnemiesWithScaledHPAroundEnemyEnemyBehaviour;
        _fixedInvokedEnemies = invokeEnemiesOriginal._fixedInvokedEnemies;
        _randomInvokedEnemies = invokeEnemiesOriginal._randomInvokedEnemies;
        _randomInvokePossibleAmmounts = invokeEnemiesOriginal._randomInvokePossibleAmmounts;
        _invokingAnim = new(EnemyControl.Animator, invokeEnemiesOriginal._invokingAnim);
        _invokeFrame = invokeEnemiesOriginal._invokeFrame;
        _invokationPattern = invokeEnemiesOriginal._invokationPattern;

        _invokingAnim.Events.Add(new(null, _invokeFrame, InvokeEnemies));
        _invokingAnim.Events.Add(new(null, _invokingAnim.Frames.Length -1, KillBehaviour));
        EnemyControl.Animator.AddAnimations(new() { _invokingAnim });

    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        EnemyControl.Animator.ChangeAnim(_invokingAnim.AnimationName);
    }

    void InvokeEnemies()
    {
        List<EnemyData> invokingEnemies = new();
        if (_fixedInvokedEnemies.Count > 0)
            invokingEnemies.AddRange(GetFixedInvokations());
        if(_randomInvokedEnemies.Count > 0)
            invokingEnemies.AddRange(GetRandomInvokations());

        int enemyCount = invokingEnemies.Count;
        invokingEnemies = Utility.ShuffleList(invokingEnemies);
        Vector2[] enemyPositions = _invokationPattern.GetPositions(EnemyControl.transform.position, enemyCount).ToArray();
        for (int i = 0; i < enemyCount; i++)
        {
            EnemyData generatedEnemy = invokingEnemies[i];
            Vector2 tilePos = new((int)enemyPositions[i].x, (int)enemyPositions[i].y);
            if (!MapManager.mm.GenerationHandler.TileMatrix.ContainsKey(tilePos))
                continue;
            var tilesInPos = MapManager.mm.GenerationHandler.TileMatrix[tilePos];
            if (tilesInPos[0].IsWall || !tilesInPos[0].IsLoaded)
                continue;
            var generatingTile = tilesInPos[0];
            EnemySpawnManager.esm.SpawnEnemy(generatingTile, generatedEnemy);

        }
    }

    List<EnemyData> GetFixedInvokations()
    {
        return _fixedInvokedEnemies;
    }

    List<EnemyData> GetRandomInvokations()
    {
        int randomEnemyCount = _randomInvokePossibleAmmounts.Count == 0 ? 1 : Utility.GetRouletteElement(_randomInvokePossibleAmmounts);
        List<EnemyData> result = new();
        for(int i = 0; i < randomEnemyCount; i++)
        {
            result.Add(Utility.GetRouletteElement(_randomInvokedEnemies));
        }
        return result;
    }

    public override void KillBehaviour()
    {
        base.KillBehaviour();
        EnemyControl.Animator.EndAnimation(_invokingAnim.AnimationName);
    }
}
