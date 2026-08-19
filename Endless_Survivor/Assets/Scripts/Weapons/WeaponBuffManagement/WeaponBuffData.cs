using System.Collections.Generic;
using UnityEngine;
using BuffDurationType = WeaponBuffHandler.BuffDurationType;

[CreateAssetMenu(fileName = "New WeaponBuff", menuName = "ScriptableObjects/WeaponBuff", order = 20)]
public class WeaponBuffData : ScriptableObject
{
    [SerializeField] int _buffMaxStacks;
    [SerializeField] WeaponStats _statsBuff;
    [SerializeField] BuffDurationType _durationType;
    [SerializeField] int _enemyKillsNeeded;
    [SerializeField] float _timeDuration;
    [SerializeField] ParticleSystem _buffParticleSystem;
    //ADD SFX AND PLAYER GFX CHANGES!!!+
    [SerializeField] PlayerGFXChanger _onBuffPlayerGfxChanger;
    [SerializeField] SFXInfo _onBuffSFX;
    //ADD A DAMAGE TYPE FILTER!!

    public int BuffMaxStacks { get { return _buffMaxStacks; } }
    public WeaponStats StatsBuff { get { return _statsBuff; } }
    public BuffDurationType DurationType { get { return _durationType; } }
    public int EnemyKillsNeeded { get { return _enemyKillsNeeded; } }
    public float TimeDuration { get { return _timeDuration; } }
    public ParticleSystem BuffParticleSystem { get { return _buffParticleSystem; } }
    public PlayerGFXChanger OnBuffPlayerGfxChanger { get { return _onBuffPlayerGfxChanger; } }
    public SFXInfo OnBuffSFX { get { return _onBuffSFX; } }
}
