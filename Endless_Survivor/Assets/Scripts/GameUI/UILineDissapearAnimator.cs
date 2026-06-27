using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class UILineDissapearAnimator : MonoBehaviour
{
    [SerializeField] UILine _line;
    [SerializeField] float _lineDisappearTime;
    float _vertexMoveSpeed;

    float _currVertexTargetDist = -1;
    float _currVertexLapsedDist = 0;

    [SerializeField] float _startDelay = .5f;
    float _startDelayTimer = 0;

    Action _onOneVertexLeft;

    public Action OnOneVertexLeft { get { return _onOneVertexLeft; } set { _onOneVertexLeft = value; } }

    private void Update()
    {
        List<Vector2> vertices = new List<Vector2>(_line.LineVertices);
        if(vertices.Count > 1)
        {
            if(_startDelayTimer > 0)
            {
                _startDelayTimer -= Time.deltaTime;
                return;
            }
            _vertexMoveSpeed = _line.LineLength / _lineDisappearTime;
            _vertexMoveSpeed = Mathf.Clamp(_vertexMoveSpeed, 1000, _vertexMoveSpeed);
            Vector2 lineMovement = vertices[1] - vertices[0];
            if(_currVertexTargetDist == -1)
                _currVertexTargetDist = lineMovement.magnitude;

            Vector2 lineMoveDir = lineMovement.normalized;
            Vector2 deltaMovement = lineMoveDir * _vertexMoveSpeed * Time.deltaTime;
            _line.LineVertices[0] += deltaMovement;
            _currVertexLapsedDist += deltaMovement.magnitude;

            if(_currVertexLapsedDist > _currVertexTargetDist)
            {
                //Delete vertex
                _line.LineVertices.RemoveAt(0);
                //reset target dist and lapsed dist
                _currVertexTargetDist = -1;
                _currVertexLapsedDist = 0;
            }
            _line.SetAllDirty();
        }
        else
        {
            if(_startDelayTimer <= 0)
            {
                _onOneVertexLeft?.Invoke();
                _startDelayTimer = _startDelay;
            }
        }
    }
}
