using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _playerStateMachine;
    bl_Joystick _joystick;
    TouchControls _touchControls;

    private void Start()
    {
        _touchControls = FindObjectOfType<TouchControls>();
        _joystick = FindObjectOfType<bl_Joystick>();
        if (GameManager.gm.UsingCustomControls)
        {
            _joystick.gameObject.SetActive(false);
            _touchControls.gameObject.SetActive(true);
        }
        else
        {
            _joystick.gameObject.SetActive(true);
            _touchControls.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        //if (Input.GetKey(KeyCode.W))
        //input += Vector2.up;
        //if (Input.GetKey(KeyCode.D))
        //input += Vector2.right;
        //if (Input.GetKey(KeyCode.S))
        //input += Vector2.down;
        //if (Input.GetKey(KeyCode.A))
        //input += Vector2.left;

        if (GameManager.gm.UsingCustomControls)
            input = _touchControls.DraggingDirection;
        else
            input = new Vector2(_joystick.Horizontal, _joystick.Vertical);
        _playerStateMachine.Movement = input.normalized;
    }
}