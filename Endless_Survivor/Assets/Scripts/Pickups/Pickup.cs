using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    List<PickupVariableBase> _variables = new List<PickupVariableBase>();
    PickupData _pickupData;
    [SerializeField] PickupControl _pickupControl;
    public PickupData PickupData { set {  _pickupData = value; } }
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
        _pickupData.PickUp(_pickupControl);
        Destroy(gameObject);
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