using KahuInteractive.HassleFreeConfig;
using UnityEditor.EditorTools;
using UnityEngine;

namespace MarbleBash
{

    [CreateAssetMenu(fileName = "Movement Config", menuName = "Configuration/Movement")]
    public class MovementConfig : ConfigBase
    {
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

    }

}
