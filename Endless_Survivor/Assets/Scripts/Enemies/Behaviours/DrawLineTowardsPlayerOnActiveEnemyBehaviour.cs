using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLineTowardsPlayerOnActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] LineXConfig _lineConfig;
    [SerializeField] float _startDissapearingDelay;
    [SerializeField] float _lineLength;
    LineXInfo _drawnLineInfo;
    float _delayTimer;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var drawLineOriginal = original as DrawLineTowardsPlayerOnActiveEnemyBehaviour;
        _lineConfig = drawLineOriginal._lineConfig;
        _startDissapearingDelay = drawLineOriginal._startDissapearingDelay;
        _lineLength = drawLineOriginal._lineLength;
        _delayTimer = _startDissapearingDelay;
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if (_drawnLineInfo == null)
        {
            Vector2 direction = PlayerControl.pc.transform.position - EnemyControl.transform.position;
            direction = direction.normalized;
            LineXConfig contextConfig = new(_lineConfig, EnemyControl.transform.position, direction, _lineLength, null);
            _drawnLineInfo = LineXManager.lm.DrawLine(contextConfig);
            if (_lineConfig.DissapearOnStart)
            {
                _drawnLineInfo = null;
                KillBehaviour();
            }
        }
        _delayTimer -= Time.deltaTime;
        if (_delayTimer <= 0)
        {
            LineXManager.lm.StartLineDissapearing(_drawnLineInfo);
            _delayTimer = _startDissapearingDelay;
            _drawnLineInfo = null;
            KillBehaviour();
        }
    }
}
