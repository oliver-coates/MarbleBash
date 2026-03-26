using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References:")]
    [SerializeField] private PlayerLook _playerLook;
    private Rigidbody _rb;


    #region Input
    [Header("Input:")]
    [SerializeField] private InputActionAsset _inputActions;
    private InputAction _moveAction;
    private void SetupInput()
    {
        InputActionMap map = _inputActions.FindActionMap("Player");

        _moveAction = map.FindAction("Move");
        map.FindAction("Jump").performed += AttemptJump;
    }

    #endregion


    [Header("Settings:")]
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    [Header("State:")]
    [SerializeField] private LayerMask _groundedLayerMask;
    [SerializeField] private bool _isGrounded;
    public bool isGrounded
    {
        get
        {
            return _isGrounded;
        }
    }


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        SetupInput();
    }

    private void Update()
    {
        _isGrounded = CheckIsGrounded();

        MoveHorizontally();
    }

    private bool CheckIsGrounded()
    {
        float halfScale = transform.localScale.x / 2f;
        Vector3 groundBoxPosition = transform.position + (Vector3.down * halfScale);
        Vector3 goundBoxSize = new Vector3(halfScale * 0.85f, 0.05f, halfScale * 0.85f);

        return Physics.CheckBox(groundBoxPosition, goundBoxSize, Quaternion.identity, _groundedLayerMask);
    }

    private void MoveHorizontally()
    {
        Vector3 movementInput = _moveAction.ReadValue<Vector2>();

        Vector3 forceThisFrame = (_playerLook.yaw * movementInput.y) + _playerLook.yawRight * movementInput.x;

        _rb.AddForce(_speed * Time.deltaTime * forceThisFrame);
    }


    private void AttemptJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }
}
