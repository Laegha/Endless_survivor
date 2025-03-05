using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _playerStateMachine;
    bl_Joystick joystick;

    private void Start()
    {
        joystick = FindObjectOfType<bl_Joystick>();
    }

    void Update()
    {
        Vector2 movement = new Vector2(joystick.Horizontal, joystick.Vertical);
        _playerStateMachine.Movement = movement.normalized;
    }
}
