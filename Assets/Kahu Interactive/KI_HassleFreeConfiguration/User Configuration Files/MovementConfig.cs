using KahuInteractive.HassleFreeAudio;
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

        [SerializeField] private float _trailIntensityMultiplier = 1;
        public float trailIntensityMultiplier
        {
            get
            {
                return _trailIntensityMultiplier;
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

        [Header("Audio:")]
        [SerializeField] private ClipSet _impactLowClipSet;
        public ClipSet impactLowClipSet
        {
            get
            {
                return _impactLowClipSet;
            }	
        }
    
        [Header("Impact decals:")]
        [SerializeField] private GameObject _impactDecalPrefab;
        public GameObject impactDecalPrefab
        {
            get
            {
                return _impactDecalPrefab;
            }	
        }
    
        [SerializeField] private float _minimumVelocityRequiredForImpactDecal = 5;
        public float minimumVelocityRequiredForImpactDecal
        {
            get
            {
                return _minimumVelocityRequiredForImpactDecal;
            }	
        }

        [SerializeField] private float _maxVelocityForImpactDecal = 40;
        public float maxVelocityForImpactDecal
        {
            get
            {
                return _maxVelocityForImpactDecal;
            }	
        }
    
        [SerializeField] private AnimationCurve _impactStartOpacityCurve;
        public AnimationCurve impactStartOpacityCurve
        {
            get
            {
                return _impactStartOpacityCurve;
            }	
        }


        [SerializeField] private float _impactDecalMinSize;
        public float impactDecalMinSize
        {
            get
            {
                return _impactDecalMinSize;
            }	
        }
    
        [SerializeField] private float _impactDecalMaxSize;
        public float impactDecalMaxSize
        {
            get
            {
                return _impactDecalMaxSize;
            }	
        }

        [SerializeField] private AnimationCurve _impactDecalSizeCurve;
        public AnimationCurve impactDecalSizeCurve
        {
            get
            {
                return _impactDecalSizeCurve;
            }	
        }
   
    }

}
