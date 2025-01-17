using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    PlayerBaseState _currState;
    PlayerStateFactory _states;

    Rigidbody _playerRb;

    Animator _playerAnimator;

    Vector2 _movement;
    PlayerStats _playerStats;


    public PlayerBaseState CurrState { get { return _currState; } set { _currState = value; } }
    public Rigidbody PlayerRb { get { return _playerRb; } }
    public Animator PlayerAnimator { get { return _playerAnimator; } }
    public Vector2 Movement { get { return _movement; } set { _movement = value; } }
    public PlayerStats PlayerStats { get { return _playerStats; } set { _playerStats = value; } }

    void Awake()
    {
        //variables definition
        _playerRb = GetComponent<Rigidbody>();
        _playerAnimator = GetComponent<Animator>();
        _playerStats = new PlayerStats(GameManager.gm.selectedCharacter.PlayerStats);

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
