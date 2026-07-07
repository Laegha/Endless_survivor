using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    readonly int _maxInstantiatedPickups = 10;
    static List<Pickup> _instantiatedPickups = new();
    List<PickupVariableBase> _variables = new List<PickupVariableBase>();
    PickupData _pickupData;
    [SerializeField] PickupControl _pickupControl;
    bool _consideredInPickupLimit = true;
    Action<Pickup> _onPickup;
    SFXInfo _onPickupSFX;
    
    public PickupData PickupData { set {  _pickupData = value; } }
    public bool ConsideredInPickupLimit { set { _consideredInPickupLimit = value; } }
    public Action<Pickup> OnPickup { get { return _onPickup; } set { _onPickup = value; } }
    public SFXInfo OnPickupSFX { set { _onPickupSFX = value; } }
    private void Start()
    {
        if (!_consideredInPickupLimit)
            return;
        _instantiatedPickups.Add(this);
        if(_instantiatedPickups.Count > _maxInstantiatedPickups)
            DestroyOldestPickup();
    }
    void DestroyOldestPickup()
    {
        var oldestPickup = _instantiatedPickups[0];
        _instantiatedPickups.Remove(oldestPickup);
        Destroy(oldestPickup.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PickUp();
    }
    public void AddVariable<T>(string variableKey, T variableValue)
    {
        PickupVariable<T> variable = new PickupVariable<T>(variableKey, variableValue);
        _variables.Add(variable);
    }
    public T GetVariable<T>(string variableKey)
    {
        PickupVariable<T> foundVariable = (PickupVariable<T>)_variables.Find(x => x.key == variableKey);
        return foundVariable != null ? foundVariable.variable : default;
    }
    public void PickUp() 
    {
        if(_pickupData != null)
            _pickupData.PickUp(_pickupControl);
        if(_consideredInPickupLimit)
            _instantiatedPickups.Remove(this);

        SoundFXManager.sm.PlaySfx(_onPickupSFX, transform.position);
        _onPickup?.Invoke(this);
    }
    private void OnDestroy()
    {
        if (!_consideredInPickupLimit)
            return;
        if (_instantiatedPickups.Contains(this))
            _instantiatedPickups.Remove(this);
    }
}

class PickupVariableBase
{
    public string key;
    public PickupVariableBase(string key)
    {
        this.key = key;
    }   
}

class PickupVariable<T> : PickupVariableBase
{
    public T variable;
    public PickupVariable(string key, T variable) : base(key) 
    {
        this.variable = variable;
    }
}