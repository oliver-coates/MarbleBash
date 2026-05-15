using System;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{
    [CreateAssetMenu(fileName = "Impact Decal Config", menuName = "Configuration/Impact Decal")]
    public class ImpactDecalConfig : ConfigBase
    {
        [Header("Min/Max velocity for a decal to appear:")]
        [SerializeField] private float _minVelocity;
        public float minVelocity
        {
            get
            {
                return _minVelocity;
            }	
        }
    
        [SerializeField] private float _maxVelocity;
        public float maxVelocity
        {
            get
            {
                return _maxVelocity;
            }	
        }
    
        [Header("Size of impact decal:")]
        [SerializeField] private float _minSize;
        public float minSize
        {
            get
            {
                return _minSize;
            }	
        }
        [SerializeField] private float _maxSize;
        public float maxSize
        {
            get
            {
                return _maxSize;
            }	
        }

        internal float GetSizeFromForce(float force)
        {
            float forceLerp = (force - minVelocity) / (maxVelocity - minVelocity);

            return Mathf.Lerp(minSize, maxSize, forceLerp);
        }
    }
}

