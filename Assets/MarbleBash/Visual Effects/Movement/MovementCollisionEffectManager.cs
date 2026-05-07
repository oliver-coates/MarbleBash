using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using Unity.Cinemachine;
using UnityEngine;

namespace MarbleBash
{
    
    public class MovementCollisionEffectManager : MarbleSubComponent
    {
        private MovementConfig _config;
        
        [Header("References:")]
        [SerializeField] private CinemachineImpulseSource _source;

        protected override void Initialise()
        {
            _config = Configuration.Get<MovementConfig>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            float collisionForce = collision.impulse.magnitude;

            string tag = collision.gameObject.tag; 
            if (tag == "Untagged")
            {
                RegisterImpactGround(collision, collisionForce);            
            }
            else if (tag == "Enemy")
            {
                RegisterImpactEnemy(collision, collisionForce);
            }
        }

        private void RegisterImpactGround(Collision c, float magnitude)
        {
            if (magnitude > _config.minimumCollisionForceRequiredForCameraShake)
            {
                DoCameraShake(magnitude);    
            }

            if (magnitude > _config.minimumVelocityRequiredForImpactDecal)
            {
                CreateImpactDecal(c, magnitude);            
            }

            AudioEngine.PlaySound(_config.impactLowClipSet, Mathf.Log(magnitude, 2f) * 0.5f);
            
        }

        private void RegisterImpactEnemy(Collision c, float magnitude)
        {
            AudioEngine.PlaySound(_config.impactLowClipSet, Mathf.Log(magnitude, 2f) * 0.5f);
        }

        private void DoCameraShake(float force)
        {
            float impulseForce = Mathf.Log(force - _config.minimumCollisionForceRequiredForCameraShake, 2f);

            impulseForce *= _config.collisionCameraShakeForceMultiplier;
            
            _source.GenerateImpulse(impulseForce);
        }
    
        private void CreateImpactDecal(Collision c, float force)
        {
            ImpactDecal decal = Instantiate(_config.impactDecalPrefab).GetComponent<ImpactDecal>();
            decal.Setup(c, force);   
        }


    }
}