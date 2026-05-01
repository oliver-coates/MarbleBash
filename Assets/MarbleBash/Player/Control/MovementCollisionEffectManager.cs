using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using Unity.Cinemachine;
using UnityEngine;

namespace MarbleBash
{
    
    public class MovementCollisionEffectManager : MonoBehaviour
    {
        private MovementConfig _config;

        private void Start()
        {
            _config = Configuration.Get<MovementConfig>();
        }

        [SerializeField] private CinemachineImpulseSource _source;

        private void OnCollisionEnter(Collision collision)
        {
            float collisionForce = collision.impulse.magnitude;            
            // float collisionForce = collision.relativeVelocity.magnitude;            

            if (collisionForce > _config.minimumCollisionForceRequiredForCameraShake)
            {
                float impulseForce = Mathf.Log(collisionForce - _config.minimumCollisionForceRequiredForCameraShake, 2f);

                impulseForce *= _config.collisionCameraShakeForceMultiplier;
                
                AudioEngine.PlaySound(_config.impactLowClipSet);

                _source.GenerateImpulse(impulseForce);
            }
        }
    }
}