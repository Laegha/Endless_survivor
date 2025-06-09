using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAttack : Attack
{
    [SerializeField] LineRenderer _lineRenderer;
    float _timer;
    Vector2 _startPosition;
    Vector2 _endPosition;
    Vector2 _shootDirection;

    float _exitSpeed = 0.5f;
    int _currExitPointIndex = 0;
    Vector2 _currExitDirection;
    float _currExitPointElapsedDist;
    float _currExitPointDist;

    public Vector2 StartPosition { get { return _startPosition; } }
    public Vector2 EndPosition { get { return _endPosition; }}
    void Update()
    {
        float deltaMovement = _exitSpeed * Time.deltaTime;
        Vector2 newPosition = (Vector2)_lineRenderer.GetPosition(_currExitPointIndex-1) + _currExitDirection * deltaMovement;
        _lineRenderer.SetPosition(_currExitPointIndex-1, newPosition);
        _currExitPointElapsedDist += deltaMovement;
        
        if(_currExitPointElapsedDist >= _currExitPointDist)
            GoToNextExitPoint();
    }
    public void Attack(float exitSpeed, int damage, RayData defaultRayData, Transform firePoint)
    {
        _startPosition = firePoint.position;

        _lineRenderer.material = defaultRayData.RayMaterial;
        _lineRenderer.startWidth = defaultRayData.RayStartWidth;
        _lineRenderer.endWidth = defaultRayData.RayEndWidth;
        
        _lineRenderer.SetPosition(0, firePoint.position);
        
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, -firePoint.right, Mathf.Infinity, Utility.GetCollidableLayers("PlayerAttack"));
        if (!hit)
        {
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, firePoint.position + -firePoint.right * 100);//if the player didn't hit nothing (which should not happen), setting the end of the ray far enough so the player can't see it
            return;

        }
        _endPosition = hit.transform.position;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _endPosition);
        EffectsHandler.TryEffects(this);

        _exitSpeed = exitSpeed;
        GoToNextExitPoint();

        EnemyControl enemyControl = hit.collider.GetComponent<EnemyControl>();
        if (enemyControl != null)
        {
            EffectsHandler.EnemyHit(enemyControl);
            enemyControl.EnemyHP.TakeDamage(damage);
        }

    }
    void GoToNextExitPoint()
    {
        _currExitPointIndex++;
        print(_lineRenderer.positionCount);
        if(_currExitPointIndex == _lineRenderer.positionCount)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 distance = _lineRenderer.GetPosition(_currExitPointIndex) - _lineRenderer.GetPosition(_currExitPointIndex -1);
        _currExitDirection = distance.normalized;
        _currExitPointDist = distance.magnitude;
        _currExitPointElapsedDist = 0;
    }
}
