using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{
    [CreateAssetMenu(fileName = "Camera Shake Config", menuName = "Configuration/Camera Shake")]
    public class CameraShakeConfig : ConfigBase
    {
        [Header("Min/Max Force:")]
        [SerializeField] private float _minimumForce;
        public float minimumForce
        {
            get
            {
                return _minimumForce;
            }	
        }
    
        [SerializeField] private float _maximumForce;
        public float maximumForce
        {
            get
            {
                return _maximumForce;
            }	
        }

        [SerializeField] private float _minimumDamage;
        public float minimumDamage
        {
            get
            {
                return _minimumDamage;
            }	
        }

        [SerializeField] private float _maximumDamage;
        public float maximumDamage
        {
            get
            {
                return _maximumDamage;
            }	
        }

        [Header("Intensity:")]
        [SerializeField] private float _damageIntensityMultiplier;
        public float damageIntensityMultiplier
        {
            get
            {
                return _damageIntensityMultiplier;
            }	
        }

        [SerializeField] private float _impactForceintensityMultiplier;
        public float impactForceintensityMultiplier
        {
            get
            {
                return _impactForceintensityMultiplier;
            }	
        }
    
        [SerializeField] private AnimationCurve _intensityCurve;
        public AnimationCurve intensityCurve
        {
            get
            {
                return _intensityCurve;
            }	
        }
    
        [Header("Distance Falloff:")]
        [SerializeField] private float _minimumRange;
        public float minimumRange
        {
            get
            {
                return _minimumRange;
            }	
        }
    
        [SerializeField] private float _maximumRange;
        public float maximumRange
        {
            get
            {
                return _maximumRange;
            }	
        }
    }
}

