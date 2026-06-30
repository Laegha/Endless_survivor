using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class PlayerInputReader : MonoBehaviour
//{
//    [SerializeField] PlayerStateMachine _playerStateMachine;
//    bl_Joystick joystick;

//    private void Start()
//    {
//        joystick = FindObjectOfType<bl_Joystick>();
//    }

//    void Update()
//    {
//        Vector2 movement = new Vector2(joystick.Horizontal, joystick.Vertical);
//        _playerStateMachine.Movement = movement.normalized;
//    }
//}

//public class PlayerInputReader : MonoBehaviour
//{
//    [SerializeField] PlayerStateMachine _playerStateMachine;

//    private void Update()
//    {
//        Vector2 input = Vector2.zero;
//        if (Input.GetKey(KeyCode.W))
//            input += Vector2.up;
//        if (Input.GetKey(KeyCode.D))
//            input += Vector2.right;
//        if (Input.GetKey(KeyCode.S))
//            input += Vector2.down;
//        if (Input.GetKey(KeyCode.A))
//            input += Vector2.left;
//        _playerStateMachine.Movement = input.normalized;
//    }
//}

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _playerStateMachine;
    TouchControls _controls;

    private void Start()
    {
        _controls = FindObjectOfType<TouchControls>();
    }

    void Update()
    {
        Vector2 movement = _controls.DraggingDirection;
        _playerStateMachine.Movement = movement.normalized;
    }
}