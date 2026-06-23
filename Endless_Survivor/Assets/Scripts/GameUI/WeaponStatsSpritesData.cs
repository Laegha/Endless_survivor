using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DamageType = DamageInfo.DamageType;

[CreateAssetMenu(fileName = "New WeaponStatsSprites", menuName = "ScriptableObjects/UI/WeaponStatsSprites", order =1)]
public class WeaponStatsSpritesData : ScriptableObject
{
    [InspectorLabel("Base stats")]
    [SerializeField] Sprite _rangeSprite;
    [SerializeField] Sprite _damageSprite;
    [SerializeField] Sprite _attackSpeedSprite;
    [SerializeField] Sprite _knockbackSprite;

    [InspectorLabel("Damage types")]
    [SerializeField] Sprite _explosiveSprite;
    [SerializeField] Sprite _fireSprite;
    [SerializeField] Sprite _chemicalSprite;
    [SerializeField] Sprite _iceSprite;
    [SerializeField] Sprite _electricSprite;
    [SerializeField] Sprite _cuttingSprite;
    [SerializeField] Sprite _laserSprite;
    
    public Sprite RangeSprite { get { return _rangeSprite; } }
    public Sprite DamageSprite { get { return _damageSprite; } }
    public Sprite AttackSpeedSprite { get { return _attackSpeedSprite; } }
    public Sprite KnockbackSprite { get { return _knockbackSprite; } }

    public Dictionary<DamageType, Sprite> DamageTypesSprites { get
        {
            Dictionary<DamageType, Sprite> result = new()
            {
                {DamageType.Explosive, _explosiveSprite }
            };
            return result;
        } }
}
