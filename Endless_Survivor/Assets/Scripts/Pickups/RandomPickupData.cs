using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RandomFromGroup Pickup", menuName = "ScriptableObjects/Pickups/RandomFromGroup", order = 0)]
public class RandomPickupData : PickupData
{
    [SerializeField] PickupData[] _possiblePickupDatas;
    public override void TransferData(GameObject pickupGameObject)
    {
        base.TransferData(pickupGameObject);
        _possiblePickupDatas[Random.Range(0, _possiblePickupDatas.Length - 1)].TransferData(pickupGameObject);
    }
}
