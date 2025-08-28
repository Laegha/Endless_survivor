using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CallEventOnTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent _onTriggerEvent;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _onTriggerEvent.Invoke();
    }
}
