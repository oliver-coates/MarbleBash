using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private Transform _yawDirection;
    public Vector3 yaw
    {
        get
        {
            return _yawDirection.forward;
        }
    }
    [SerializeField] private Transform _pitchDirection;
    public Vector3 pitch
    {
        get
        {
            return _pitchDirection.forward;
        }
    }


    #region Input
    [Header("Input:")]
    [SerializeField] private InputActionAsset _inputActions;
    private InputAction _lookAction;
    private void SetupInput()
    {
        InputActionMap map = _inputActions.FindActionMap("Player");

        _lookAction = map.FindAction("Look");
    }
    #endregion

    [Header("Settings:")]
    [SerializeField] private float _lookSensitivity;

    private void Start()
    {
        SetupInput();
    }

    private void Update()
    {
        // Follow player
        transform.position = _playerTransform.transform.position;

        // Apply rotations
        Vector2 lookInput = _lookAction.ReadValue<Vector2>();

        lookInput *= _lookSensitivity * Time.deltaTime;

        _yawDirection.Rotate(0, lookInput.x, 0);
        _pitchDirection.Rotate(-lookInput.y, 0, 0);
    }

}
