using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    [CreateAssetMenu(fileName = "Movement Config", menuName = "Configuration/Movement")]
    public class MovementConfig : ConfigBase
    {
        [Header("Wall Jump:")]
        [SerializeField] private float _wallJumpForceMultiplier = 1f;
        public float wallJumpForceMultiplier
        {
            get
            {
                return _wallJumpForceMultiplier;
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
        [SerializeField] private float _minimumVelocityRequiredForImpactDecal = 5;
        public float minimumVelocityRequiredForImpactDecal
        {
            get
            {
                return _minimumVelocityRequiredForImpactDecal;
            }	
        }
   
    }

}
