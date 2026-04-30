using System;
using KahuInteractive.HassleFreeConfig;
using MarbleBash;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private MovementConfig _config;

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
    [SerializeField] private LayerMask _groundedLayerMask;

    [Header("State:")]
    [SerializeField] private bool _isGrounded;
    public bool isGrounded
    {
        get
        {
            return _isGrounded;
        }
    }
    
    [SerializeField] private bool _isAgainstWall;
    public bool isAgainstWall
    {
        get
        {
            return _isAgainstWall;
        }	
    }
  
    [SerializeField] private Vector3 _wallNormal;
    public Vector3 wallNormal
    {
        get
        {
            return _wallNormal;
        }	
    }

    /// <summary>
    /// The position on the surface directly below the player. 
    /// </summary>
    private Vector3 _groundedPosition;
    public Vector3 groundedPosition
    {
        get
        {
            return _groundedPosition;
        }
    }


    private void Start()
    {
        _config = Configuration.Get<MovementConfig>();
        _rb = GetComponent<Rigidbody>();

        SetupInput();
    }

    private void Update()
    {
        UpdateGroundedState();

        MoveHorizontally();
    }

    private void UpdateGroundedState()
    {
        // Check to see if we are grounded:
        float halfScale = transform.localScale.x / 2f;
        Vector3 groundBoxPosition = transform.position + (Vector3.down * halfScale);
        Vector3 goundBoxSize = new Vector3(halfScale * 0.5f, 0.05f, halfScale * 0.5f);

        _isGrounded = Physics.CheckBox(groundBoxPosition, goundBoxSize, Quaternion.identity, _groundedLayerMask);
        
        // Update our grounded position:
        Ray downRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(downRay, out RaycastHit hit, 100f, _groundedLayerMask))
        {
            _groundedPosition = hit.point;
        }
    }



    private void MoveHorizontally()
    {
        Vector3 movementInput = _moveAction.ReadValue<Vector2>();

        Vector3 forceThisFrame = (_playerLook.yawForward * movementInput.y) + _playerLook.yawRight * movementInput.x;

        _rb.AddForce(_speed * Time.deltaTime * forceThisFrame);
    }

    private void AttemptJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            Jump();
        }
        else if (_isAgainstWall)
        {
            WallJump();    
        }
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up  * _config.jumpForceMultiplier, ForceMode.VelocityChange);
    }

    private void WallJump()
    {
        Vector3 direction = (_wallNormal + Vector3.up).normalized;

        Vector3 velocity = _rb.linearVelocity;
        if (velocity.y < 0)
        {
            velocity.y = velocity.y / 2f;
            _rb.linearVelocity = velocity;
        } 
        _rb.AddForce(direction  * _config.wallJumpForceMultiplier, ForceMode.VelocityChange);

        _isAgainstWall = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isGrounded == false && IsObjectOnGroundedLayer(collision.gameObject))
        {
            if (IsCollisionAgainstWall(collision))
            {
                _isAgainstWall = true;        
                _wallNormal = collision.contacts[0].normal;    
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if ((_groundedLayerMask.value & (1 << collision.gameObject.layer)) > 0)
        {
            _isAgainstWall = false;
        }
    }

    private bool IsObjectOnGroundedLayer(GameObject obj)
    {
        return ((_groundedLayerMask.value & (1 << obj.layer)) > 0); 
    }

    private bool IsCollisionAgainstWall(Collision collision)
    {
        // Find difference in height between the player and the point hit
        float yDiff = Mathf.Abs(transform.position.y - collision.contacts[0].point.y);

        // We consider against a wall if the difference in y is less than 0.2 units.
        return yDiff < 0.2f; 
    }
}
