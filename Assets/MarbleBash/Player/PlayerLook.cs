using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private Transform _yawDirection;
    [SerializeField] private CinemachineCamera _camera;

    private float _pitchDegrees;

    public Vector3 yaw
    {
        get
        {
            return _yawDirection.forward;
        }
    }
    public Vector3 yawRight
    {
        get
        {
            return _yawDirection.right;
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

    [Header("Settings / Sensitivity:")]
    [SerializeField] private float _lookSensitivity;

    [Header("Settings / Field of View:")]
    [SerializeField] private float _minFov;
    [SerializeField] private float _maxFov;
    [SerializeField] private float _maxVelocityFovDistortion;
    private float _currentFov;
    [Range(0,1), SerializeField] private float _fovChangeRate;

    [Header("Settings / Pitch:")]
    [SerializeField] private float _minimumPitch = -5f;
    [SerializeField] private float _maxmimumPitch = 70f;

    private void Start()
    {
        SetupInput();
        LockCursor();
        SetupCameraFov();
    }

    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Follow player
        transform.position = _playerTransform.transform.position;
        
        RotateAroundPlayerByInput();

        RecaclulateCameraFov(_playerRB.linearVelocity.magnitude);
    }

    private void RotateAroundPlayerByInput()
    {
        Vector2 lookInput = _lookAction.ReadValue<Vector2>();
        lookInput *= _lookSensitivity * Time.deltaTime;

        // Yaw is simple:
        _yawDirection.Rotate(0, lookInput.x, 0);
        
        // Pitch requiires clamping:
        _pitchDegrees += -lookInput.y;
        _pitchDegrees = Mathf.Clamp(_pitchDegrees, _minimumPitch, _maxmimumPitch);
        _pitchDirection.localRotation = Quaternion.Euler(_pitchDegrees, 0f, 0f);

    }

    private void RecaclulateCameraFov(float currentVelocity)
    {
        float velocityT = currentVelocity / _maxVelocityFovDistortion;

        float targetFov = Mathf.Lerp(_minFov, _maxFov, velocityT);

        _currentFov = Mathf.Lerp(_currentFov, targetFov, Time.deltaTime * _fovChangeRate);

        _camera.Lens.FieldOfView = _currentFov; 
    }

    private void SetupCameraFov()
    {
        _currentFov = _minFov;
    }

}
