using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] string _targetTag;
    Transform _target;
    Rigidbody2D _rb;

    void Start()
    {
        _target = GameObject.FindWithTag(_targetTag).transform;
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 direction = (_target.position - transform.position).normalized;
        _rb.velocity = direction * _moveSpeed;
    }
}
