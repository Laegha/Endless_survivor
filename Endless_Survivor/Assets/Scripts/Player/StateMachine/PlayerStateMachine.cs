using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerBaseState _currState;
    PlayerStateFactory _states;

    [SerializeField] PlayerControl _playerControl;

    Vector2 _movement;

    public PlayerBaseState CurrState { get { return _currState; } set { _currState = value; } }
    public PlayerControl PlayerControl { get { return _playerControl; } }
    public Vector2 Movement { get { return _movement; } set { _movement = value; } }

    void Awake()
    {
        //states initialization
        _states = new PlayerStateFactory(this);
        _currState = _states.Idle();
        _currState.EnterState();
    }

    void Update()
    {
        _currState.UpdateStates();
    }

    private void OnCollisionEnter(Collision collision)
    {
        _currState.OnCollisionEnter(collision);   
    }

}
