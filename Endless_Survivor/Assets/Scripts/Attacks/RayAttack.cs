using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayAttack : Attack
{
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] SpriteRenderer _startSpriteRenderer;

    Vector2 _startPosition;
    Vector2 _endPosition;

    float _exitSpeed = 0.5f;
    int _currExitPointIndex = 0;
    Vector2 _currExitDirection;
    float _currExitPointElapsedDist;
    float _currExitPointDist;
    new public LineAttackGfxInterface AttackGfxInterface => new LineAttackGfxInterface();
    public override AttackEffectArea AttackEffectArea
    {
        get
        {
            return new AttackEffectArea(AttackEffectArea.IAttackEffectAreaType.Vector, _startPosition, _endPosition, false);
        }
    }
    void Update()
    {
        float deltaMovement = _exitSpeed * Time.deltaTime;
        Vector2 newPosition = (Vector2)_lineRenderer.GetPosition(_currExitPointIndex-1) + _currExitDirection * deltaMovement;
        _lineRenderer.SetPosition(_currExitPointIndex-1, newPosition);
        _startSpriteRenderer.transform.position = newPosition;
        _currExitPointElapsedDist += deltaMovement;
        
        if(_currExitPointElapsedDist >= _currExitPointDist)
            GoToNextExitPoint();
    }
    public void Attack(int damage, float knockback, RayData defaultRayData, Vector2 position, Vector2 direction, List<Collider2D> ignoreColliders = null)
    {
        AttackDamage = damage;
        _startPosition = position;

        SetGFX(defaultRayData.RayMaterial, defaultRayData.RayStartWidth, defaultRayData.RayEndWidth, defaultRayData.RayStartSprite);
        
        _lineRenderer.SetPosition(0, position);

        var hitObjs = Physics2D.RaycastAll(position, direction, Mathf.Infinity, Utility.GetCollidableLayers("PlayerAttack"));
        if (hitObjs.Length == 0)
        {
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, position + direction * 100);//if the player didn't hit nothing (which should not happen), setting the end of the ray far enough so the player can't see it
            return;

        }
        List<RaycastHit2D>filteredHitObjs = new (hitObjs.ToList());
        if(ignoreColliders != null)
            foreach (var hitObj in hitObjs)
            {
                if(!ignoreColliders.Contains(hitObj.collider))
                    continue;
                filteredHitObjs.Remove(hitObj);
            }

        int hitObjsAmmount = Mathf.Min(AttackPiercing, filteredHitObjs.Count - 1);
        RaycastHit2D lastHit = filteredHitObjs[hitObjsAmmount];
        _endPosition = lastHit.transform.position;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _endPosition);
        EffectsHandler.TryEffects(this);

        _exitSpeed = defaultRayData.RayExitSpeed;
        GoToNextExitPoint();

        for(int i = 0; i <= hitObjsAmmount; i++)
        {
            var hit = filteredHitObjs[i];
            EnemyControl enemyControl = Utility.FindFirstComponentInParent<EnemyControl>(hit.collider.gameObject);
            if (enemyControl != null)
            {
                enemyControl.EnemyHP.TakeDamage(AttackDamage, direction, knockback);
                EffectsHandler.EnemyHit(enemyControl);
            }

        }

    }
    void GoToNextExitPoint()
    {
        _currExitPointIndex++;
        if(_currExitPointIndex == _lineRenderer.positionCount)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 distance = _lineRenderer.GetPosition(_currExitPointIndex) - _lineRenderer.GetPosition(_currExitPointIndex -1);
        _currExitDirection = distance.normalized;
        _currExitPointDist = distance.magnitude;
        _currExitPointElapsedDist = 0;

        float startRotation = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg -90;
        _startSpriteRenderer.transform.rotation = Quaternion.Euler(0, 0, startRotation);
    }
    public override void ChangeGfx(AttackGfxInterface gfxInterface)
    {
        LineAttackGfxInterface lineAttackGfxInterface = gfxInterface as LineAttackGfxInterface;
        lineAttackGfxInterface.ApplyGfx(_lineRenderer);
    }
    void SetGFX(Material lineMaterial, float startWidth, float endWidth, Sprite startSprite)
    {
        _lineRenderer.material = lineMaterial;
        _lineRenderer.startWidth = startWidth;
        _lineRenderer.endWidth = endWidth;
        _startSpriteRenderer.sprite = startSprite;
    }
}
