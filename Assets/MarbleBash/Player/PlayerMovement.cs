using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private PlayerLook _playerLook;

    [SerializeField] private float _speed;

    #region Input
    [Header("Input:")]
    [SerializeField] private InputActionAsset _inputActions;
    private InputAction _moveAction;
    private void SetupInput()
    {
        InputActionMap map = _inputActions.FindActionMap("Player");

        _moveAction = map.FindAction("Move");
    }
    #endregion

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        SetupInput();
    }

    private void Update()
    {   
        Vector3 movementInput = _moveAction.ReadValue<Vector2>();

        Vector3 forceThisFrame = _playerLook.yaw * movementInput.y;

        _rb.AddForce(_speed * Time.deltaTime * forceThisFrame);
    }

}
