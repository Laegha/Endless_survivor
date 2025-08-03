using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTransformerTesting : MonoBehaviour
{
    [SerializeField] PickupData _pickupData;

    private void Start()
    {
        _pickupData.TransferData(GetComponent<PickupControl>());
    }
}
