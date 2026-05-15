using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{
    [DefaultExecutionOrder(-5)]
    public abstract class MarbleMovement : MarbleSubComponent
    {
        protected MovementConfig _config;


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

        [Header("Additional:")]
        public Vector3 lookDirection
        {
            get
            {
                return GetLookDirection();
            }
        }

        // Settings:
        protected float _moveSpeedMultiplier;
        protected float _jumpHeightMultiplier;

        protected abstract bool CheckIsGrounded(out float distanceToGround);
        protected abstract Vector3 GetLookDirection();
        protected abstract Vector3 GetMovementDirection();
        



        protected override void Initialise()
        {            
            _config = Configuration.Get<MovementConfig>();
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
            Vector3 jumpForce = Vector3.up * _jumpHeightMultiplier * _marble.stats.jumpHeight.value;
        
            _marble.rigidbody.AddForce(jumpForce);
        }
        
        protected void Move()
        {
            Vector3 movementInput = GetMovementDirection();

            float speedMultiplier = _marble.stats.movementSpeed.value * _moveSpeedMultiplier * Time.deltaTime;

            _marble.rigidbody.AddForce(speedMultiplier * movementInput);
        }

    }



}

