using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{
    [DefaultExecutionOrder(-5)]
    public abstract class MarbleMovement : MarbleSubComponent
    {
        protected MovementConfig _config;
        public const float GROUNDED_RAYCAST_DOWN_MAXIMUM_DISTANCE = 999f;


        [Header("Velocity:")]
        [SerializeField] private Vector3 _velocity;
        public Vector3 velocity
        {
            get
            {
                return _velocity;
            }	
        }

        [SerializeField] private float _speed;
        public float speed
        {
            get
            {
                return _speed;
            }	
        }

        [Header("Cached velocity:")]
        [SerializeField] private Vector3 _cachedVelocity;
        public Vector3 cachedVelocity
        {
            get
            {
                return _cachedVelocity;
            }	
        }

        [SerializeField] private float _cachedSpeed;
        public float cachedSpeed
        {
            get
            {
                return _cachedSpeed;
            }	
        }

        [Header("Grounded:")]
        [SerializeField] private bool _isGrounded;
        public bool isGrounded
        {
            get
            {
                return _isGrounded;
            }	
        }

        [SerializeField] private float _distanceToGround;
        public float distanceToGround
        {
            get
            {
                return _distanceToGround;
            }	
        }

        /// <summary>
        /// The position on the surface directly below the marble.
        /// Not updated when over no geometry, so will also act as the last position the marble was on the ground. 
        /// </summary>
        protected Vector3 _groundedPosition;
        public Vector3 groundedPosition
        {
            get
            {
                return _groundedPosition;
            }
        }

        [Header("Additional:")]
        public Vector3 lookDirection
        {
            get
            {
                return GetLookDirection();
            }
        }

        #region Configuration Values
        // These two tuners are set by the subclasses, as they need to be different depending on if we are a player or enemy.
        protected float _moveSpeedMultiplier;
        protected float _jumpHeightMultiplier;
        protected float _moveSpeedBase;
        protected float _jumpHeightBase;

        #endregion

        protected abstract bool CheckIsGrounded(out float distanceToGround);
        protected abstract Vector3 GetLookDirection();
        protected abstract Vector3 GetMovementDirection();


        protected override void Initialise()
        {            
            _config = Configuration.Get<MovementConfig>();
            _distanceToGround = 1f;

            _moveSpeedBase = Configuration.Read("marble_move_speed_base");
            _jumpHeightBase = Configuration.Read("marble_jump_height_base");
        }

        protected virtual void LateUpdate()
        {
            _cachedVelocity = _marble.rigidbody.linearVelocity;
            _cachedSpeed = _cachedVelocity.magnitude;
        }

        protected virtual void Update()
        {
            UpdateVelocityAndSpeed();

            _isGrounded = CheckIsGrounded(out _distanceToGround);
        
            Move();
        }

        private void UpdateVelocityAndSpeed()
        {
            _velocity = _marble.rigidbody.linearVelocity;
            _speed = _velocity.magnitude;
        }

        protected bool IsObjectOnGroundedLayer(GameObject obj)
        {
            return ((_config.groundedLayerMask.value & (1 << obj.layer)) > 0); 
        }

        protected void Jump()
        {
            float jumpForce = _jumpHeightBase * _jumpHeightMultiplier * _marble.stats.jumpHeight.value;
            
            Vector3 jumpVelocity = Vector3.up * jumpForce;
        
            _marble.rigidbody.AddForce(jumpVelocity);
        }
        
        protected void Move()
        {
            Vector3 movementInput = GetMovementDirection();

            float speedMultiplier = _moveSpeedBase *  _moveSpeedMultiplier * _marble.stats.movementSpeed.value * Time.deltaTime;

            _marble.rigidbody.AddForce(speedMultiplier * movementInput);
        }

    }



}

