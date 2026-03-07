using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveToRandomFixedDirWhileActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<Vector2> _possibleDirections = new List<Vector2>();
    [SerializeField] DirectionalCustomAnimation _movingAnimation;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenMovements;
    [SerializeField] RandomBetweenTwoConstants _movementDistance;
    [SerializeField] RandomBetweenTwoConstants _speed;

    bool _isMoving;
    float _timer;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var moveRandomOriginal = original as MoveToRandomFixedDirWhileActiveEnemyBehaviour;
        _possibleDirections = new(moveRandomOriginal._possibleDirections);
        _movingAnimation = new(EnemyControl.Animator, moveRandomOriginal._movingAnimation);
        _timeBetweenMovements = moveRandomOriginal._timeBetweenMovements;
        _movementDistance = moveRandomOriginal._movementDistance;
        _speed = moveRandomOriginal._speed;
        _timer = _timeBetweenMovements.rand;

        EnemyControl.Animator.AddAnimations(_movingAnimation.NonNullAnimations);
    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if (_isMoving)
            return;
        if (_timer > 0)
            _timer -= Time.deltaTime;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if (_timer > 0)
            return;
        StartMoving();
    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        StopMoving();
    }
    void StartMoving()
    {
        _isMoving = true;
        Vector2 dir = _possibleDirections[Random.Range(0, _possibleDirections.Count)];
        float dist = _movementDistance.rand;
        Vector2 reachingTilePos = (Vector2)EnemyControl.transform.position + dir * dist;
        reachingTilePos = new Vector2((int)reachingTilePos.x, (int)reachingTilePos.y);
        while(!MapManager.mm.GenerationHandler.TileMatrix.ContainsKey(reachingTilePos) || !MapManager.mm.LoadedTiles.Any(x => (Vector2)x.transform.position == reachingTilePos) || MapManager.mm.GenerationHandler.TileMatrix[reachingTilePos][0].IsWall)
        {
            dist--;
            dist = _movementDistance.rand;
            reachingTilePos = (Vector2)EnemyControl.transform.position + dir * dist;
            reachingTilePos = new Vector2((int)reachingTilePos.x, (int)reachingTilePos.y);
        }
        float moveSpeed = _speed.rand;
        float moveTime = Mathf.Abs(dist / moveSpeed);
        EnemyControl.RbForcesController.ChangeCurrForce(new(dir, moveSpeed, 5, ForceMode2D.Impulse, moveTime));
        EnemyControl.Animator.ChangeAnim(_movingAnimation.GetAnim(dir));
        GameManager.gm.DelayAction(moveTime, StopMoving, () => !_isMoving);
    }
    void StopMoving()
    {
        _isMoving = false;
        _timer = _timeBetweenMovements.rand;
        if(_movingAnimation.NonNullAnimations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName)) 
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);
    }
}
