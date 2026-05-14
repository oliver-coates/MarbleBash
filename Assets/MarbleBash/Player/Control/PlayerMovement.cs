using System;
using KahuInteractive.HassleFreeConfig;
using KahuInteractive.VisualFX;
using MarbleBash;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MarbleMovement
{

    [Header("References:")]
    [SerializeField] private PlayerLook _playerLook;

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

    [Header("State:")]
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

    private float _speedTuner;
    private float _jumpHeightTuner;

    protected override void Initialise()
    {
        base.Initialise();

        SetupInput();

        _speedTuner = Configuration.Read("player_movement_speed");
        _jumpHeightTuner = Configuration.Read("player_jump_height");
    }


    protected override void Update()
    {
        base.Update();

        MoveHorizontally();

        VFX.UpdateRTPC("Player Speed", cachedSpeed);
    }

    private void MoveHorizontally()
    {
        Vector3 movementInput = _moveAction.ReadValue<Vector2>();

        Vector3 movementDir = (_playerLook.yawForward * movementInput.y) + _playerLook.yawRight * movementInput.x;

        float speedMultiplier = _marble.stats.movementSpeed.value * _speedTuner * Time.deltaTime;

        _marble.rigidbody.AddForce(speedMultiplier * movementDir);
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
        Vector3 jumpForce = Vector3.up * _jumpHeightTuner * _marble.stats.jumpHeight.value;
        
        _marble.rigidbody.AddForce(jumpForce);
    }

    private void WallJump()
    {
        Vector3 direction = (_wallNormal + Vector3.up).normalized;

        Vector3 velocity = _marble.rigidbody.linearVelocity;
        if (velocity.y < 0)
        {
            velocity.y = velocity.y / 2f;
            _marble.rigidbody.linearVelocity = velocity;
        } 
        _marble.rigidbody.AddForce(direction  * _config.wallJumpForceMultiplier, ForceMode.VelocityChange);

        _isAgainstWall = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGrounded == false && IsObjectOnGroundedLayer(collision.gameObject))
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
        if (IsObjectOnGroundedLayer(collision.gameObject))
        {
            _isAgainstWall = false;
        }
    }

    private bool IsCollisionAgainstWall(Collision collision)
    {
        // Find difference in height between the player and the point hit
        float yDiff = Mathf.Abs(transform.position.y - collision.contacts[0].point.y);

        // We consider against a wall if the difference in y is less than 0.2 units.
        return yDiff < 0.2f; 
    }

    protected override bool CheckIsGrounded(out float distanceToGround)
    {
        // Check to see if we are grounded:
        float halfScale = transform.localScale.x / 2f;
        
        // Find the position at the very bottom of our marble
        Vector3 floorPosition = transform.position + (Vector3.down * halfScale) + (Vector3.up * 0.05f);
        
        Vector3 goundBoxSize = new Vector3(halfScale * 0.5f, 0.15f, halfScale * 0.5f);

        bool isGrounded = Physics.CheckBox(floorPosition, goundBoxSize, Quaternion.identity, _config.groundedLayerMask);
        
        // Update our grounded position:
        Ray downRay = new Ray(floorPosition, Vector3.down);
        if (Physics.Raycast(downRay, out RaycastHit hit, 100f, _config.groundedLayerMask))
        {
            _groundedPosition = hit.point;
            distanceToGround = hit.distance;
        }
        else
        {
            distanceToGround = 100f;
        }

        return isGrounded;
    }

    protected override Vector3 GetLookDirection()
    {
        return Player.look.pitchForward;
    }
}
