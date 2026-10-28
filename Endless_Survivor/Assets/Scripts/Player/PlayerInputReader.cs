using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _playerStateMachine;
    bl_Joystick _joystick;
    DinamycJoystickHandler _dinamycJoystickHandler;
    TouchControls _touchControls;

    private void Start()
    {
        _touchControls = FindObjectOfType<TouchControls>();
        _joystick = FindObjectOfType<bl_Joystick>();
        _dinamycJoystickHandler = FindObjectOfType<DinamycJoystickHandler>();
        UpdateMobileControls();
        //_joystick.gameObject.SetActive(false);
        //_touchControls.gameObject.SetActive(false);
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

        if (GameManager.gm.SettingsHandler.ControlsType == SettingsJsonInfo.ControlsType.StaticJoystic)
            input = new Vector2(_joystick.Horizontal, _joystick.Vertical);
        else if (GameManager.gm.SettingsHandler.ControlsType == SettingsJsonInfo.ControlsType.DynamicJoystic)
            input = _dinamycJoystickHandler.InputDir;
        else
            input = _touchControls.DraggingDirection;
        _playerStateMachine.Movement = input.normalized;
    }
    public void UpdateMobileControls()
    {
        if (GameManager.gm.SettingsHandler.ControlsType == SettingsJsonInfo.ControlsType.StaticJoystic)
        {
            _joystick.gameObject.SetActive(true);
            _dinamycJoystickHandler.gameObject.SetActive(false);
            _touchControls.gameObject.SetActive(false);
        }
        else if (GameManager.gm.SettingsHandler.ControlsType == SettingsJsonInfo.ControlsType.DynamicJoystic)
        {
            _dinamycJoystickHandler.gameObject.SetActive(true);
            _joystick.gameObject.SetActive(false);
            _touchControls.gameObject.SetActive(false);
        }
        else
        {
            _dinamycJoystickHandler.gameObject.SetActive(false);
            _joystick.gameObject.SetActive(false);
            _touchControls.gameObject.SetActive(true);
        }
    }
}