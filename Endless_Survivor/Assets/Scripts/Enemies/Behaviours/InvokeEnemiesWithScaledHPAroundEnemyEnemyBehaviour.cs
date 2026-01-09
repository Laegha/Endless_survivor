using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvokeEnemiesWithScaledHPAroundEnemyEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<EnemyData>_invokedEnemies;
    [SerializeField] CustomAnimation _invokingAnim;
    [SerializeField] int _invokeFrame;
    [SerializeField] float _hpMultiplier = 1.0f;

    [SerializeReference] IPattern _invokationPattern;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var invokeEnemiesOriginal = original as InvokeEnemiesWithScaledHPAroundEnemyEnemyBehaviour;
        _invokedEnemies = invokeEnemiesOriginal._invokedEnemies;
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
        int enemyCount = _invokedEnemies.Count;
        var invokingEnemies = Utility.ShuffleList(_invokedEnemies);
        Vector2[] enemyPositions = _invokationPattern.GetPositions(EnemyControl.transform.position, enemyCount).ToArray();
        for(int i = 0; i < enemyCount; i++) 
        {
            Vector2 enemyPos = enemyPositions[i];
            GameObject enemy = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Enemy"], enemyPos, Quaternion.identity);
            invokingEnemies[i].TransferEnemyData(enemy);
            enemy.GetComponent<EnemyControl>().EnemyHP.MaxHP = (int)(EnemyControl.EnemyHP.MaxHP * _hpMultiplier);
        }

    }
}
