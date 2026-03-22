using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveRandomlyWhileActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenMovements;
    [SerializeField] RandomBetweenTwoConstants _movementDistance;
    [SerializeField] RandomBetweenTwoConstants _speed;

    bool _isMoving;
    float _timer;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var moveRandomOriginal = original as MoveRandomlyWhileActiveEnemyBehaviour;
        _timeBetweenMovements = moveRandomOriginal._timeBetweenMovements;
        _movementDistance = moveRandomOriginal._movementDistance;
        _speed = moveRandomOriginal._speed;
        _timer = _timeBetweenMovements.rand;
    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if (_isMoving)
            return;
        if(_timer  > 0)
            _timer -= Time.deltaTime;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if (_timer > 0 || _isMoving)
            return;
        StartMoving();
    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _isMoving = false;
    }
    void StartMoving()
    {
        _isMoving = true;
        Vector2 dir = Random.insideUnitCircle;
        float dist = _movementDistance.rand;
        Vector2 reachingTilePos = (Vector2)EnemyControl.transform.position + dir * dist;
        reachingTilePos = new Vector2((int)reachingTilePos.x, (int)reachingTilePos.y);
        while(!MapManager.mm.GenerationHandler.TileMatrix.ContainsKey(reachingTilePos) || !MapManager.mm.LoadedTiles.Any(x => (Vector2)x.transform.position == reachingTilePos) || MapManager.mm.GenerationHandler.TileMatrix[reachingTilePos][0].IsWall)
        {
            dir = Random.insideUnitCircle;
            dist = _movementDistance.rand;
            reachingTilePos = (Vector2)EnemyControl.transform.position + dir * dist;
            reachingTilePos = new Vector2((int)reachingTilePos.x, (int)reachingTilePos.y);
        }
        float moveSpeed = _speed.rand;
        float moveTime = dist / moveSpeed; 
        EnemyControl.RbForcesController.ChangeCurrForce(new(dir,moveSpeed, 5, ForceMode2D.Impulse, moveTime));
        GameManager.gm.DelayAction(moveTime, StopMoving, () => !_isMoving);
    }
    void StopMoving()
    {
        _isMoving = false;
        _timer = _timeBetweenMovements.rand;
    }
}
