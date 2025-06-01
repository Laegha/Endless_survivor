using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] AttackEffectsHandler _effectsHandler;
    public AttackEffectsHandler EffectsHandler { get { return _effectsHandler; } }
}
