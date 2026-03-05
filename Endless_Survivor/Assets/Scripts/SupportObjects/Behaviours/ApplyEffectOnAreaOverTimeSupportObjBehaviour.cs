using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyEffectOnAreaOverTimeSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] float _effectCooldown;
    [SerializeField] ParticleSystem _particles;
    [SerializeField] PlayerStatusEffectData _playerStatusEffect;
    [SerializeField] EnemyStatusEffectData _enemyStatusEffect;
    Dictionary<GameObject, float> _affectedObjsCooldownTimers = new Dictionary<GameObject, float>();
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        OnObjUpdateArea += ApplyEffects;
    }
    void ReduceCooldowns()
    {
        var damagedObjsCooldownTimersCopy = new Dictionary<GameObject, float>(_affectedObjsCooldownTimers);
        foreach (var key in damagedObjsCooldownTimersCopy.Keys)
        {
            _affectedObjsCooldownTimers[key] -= Time.deltaTime;
            if (_affectedObjsCooldownTimers[key] <= 0)
                _affectedObjsCooldownTimers.Remove(key);
        }
    }
    void ApplyEffects(GameObject objInArea)
    {
        EnemyControl enemyControl = Utility.FindFirstComponentInParent<EnemyControl>(objInArea);
        PlayerControl playerControl = Utility.FindFirstComponentInParent<PlayerControl>(objInArea);

        if (enemyControl == null && playerControl == null)
            return;

        GameObject affectedObj = objInArea.transform.root.gameObject;
        if (_affectedObjsCooldownTimers.ContainsKey(affectedObj))
            return;

        //if (enemyControl != null)
            //enemyControl.StatusEffectManager.AddEffects();
        //if(playerControl != null) 
            //playerControl.StatusEffectManager

        _affectedObjsCooldownTimers.Add(affectedObj, _effectCooldown);
    }
}
