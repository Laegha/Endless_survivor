using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatImage : MonoBehaviour
{
    enum WeaponStatImageType
    {
        Range,
        Damage,
        AttackSpeed,
        Knockback
    }
    [SerializeField] WeaponStatImageType _statType;
    private void Start()
    {
        Sprite newSprite = null;
        switch(_statType)
        {
            case WeaponStatImageType.Range:
                newSprite = GameManager.gm.WeaponStatsSprites.RangeSprite;
                break;
            case WeaponStatImageType.Damage:
                newSprite = GameManager.gm.WeaponStatsSprites.DamageSprite;
                break;
            case WeaponStatImageType.AttackSpeed:
                newSprite = GameManager.gm.WeaponStatsSprites.AttackSpeedSprite;
                break;
            case WeaponStatImageType.Knockback:
                newSprite = GameManager.gm.WeaponStatsSprites.KnockbackSprite;
                break;
        }
        var scalableImage = GetComponent<ScalableToTargetImage>();
        if(scalableImage != null)
        {
            scalableImage.ChangeImageSprite(newSprite);
        }
    }
}
