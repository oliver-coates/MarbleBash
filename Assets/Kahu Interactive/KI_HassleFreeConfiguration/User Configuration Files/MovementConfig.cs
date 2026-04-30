using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    [CreateAssetMenu(fileName = "Movement Config", menuName = "Configuration/Movement")]
    public class MovementConfig : ConfigBase
    {
        [Header("Jump:")]
        [SerializeField] private float _jumpForceMultiplier = 1f;
        public float jumpForceMultiplier
        {
            get
            {
                return _jumpForceMultiplier;
            }	
        }

        [Header("Wall Jump:")]
        [SerializeField] private float _wallJumpForceMultiplier = 1f;
        public float wallJumpForceMultiplier
        {
            get
            {
                return _wallJumpForceMultiplier;
            }	
        }

        [Header("Camera Shake:")]
        [SerializeField] private float _collisionCameraShakeForceMultiplier;
        public float collisionCameraShakeForceMultiplier
        {
            get
            {
                return _collisionCameraShakeForceMultiplier; 
            }	
        }

        [SerializeField] private float _minimumCollisionForceRequiredForCameraShake;
        public float minimumCollisionForceRequiredForCameraShake
        {
            get
            {
                return _minimumCollisionForceRequiredForCameraShake;
            }	
        }

        [Header("Movement Trail:")]
        [SerializeField] private float _minimumVelocityForTrail = 10;
        public float minimumVelocityForTrail
        {
            get
            {
                return _minimumVelocityForTrail;
            }	
        }
    
        [SerializeField] private float _maxVelocityForTrail = 20;
        public float maxVelocityForTrail
        {
            get
            {
                return _maxVelocityForTrail;
            }	
        }

        [SerializeField] private float _trailIntensityChangeTightness = 1;
        public float trailIntensityChangeTightness
        {
            get
            {
                return _trailIntensityChangeTightness;
            }	
        }
    }

}
