using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbForcesController : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    RbForceInfo _currForceInfo;
    float _impulseForceTimer = 0;
    void Update()
    {
        if (_currForceInfo == null)
            return;

        _rb.velocity = _currForceInfo.direction * _currForceInfo.strength;
        if (_currForceInfo.forceMode != ForceMode2D.Impulse)
            return;

        _impulseForceTimer -= Time.deltaTime;
        if (_impulseForceTimer > 0)
            return;

        Stop();
    }
    public void ChangeCurrForce(RbForceInfo force)
    {
        if (_currForceInfo != null && _currForceInfo.priority > force.priority)
            return;
        _currForceInfo = force;
        if(force.forceMode == ForceMode2D.Impulse)
            _impulseForceTimer = force.impulseTime;
    }
    public void Stop()
    {
        _currForceInfo = null;
        ChangeCurrForce(new(Vector2.zero, 0, 0, ForceMode2D.Force));
    }
}
