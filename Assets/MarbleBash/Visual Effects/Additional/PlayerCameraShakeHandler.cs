using System;
using KahuInteractive.HassleFreeAudio;
using KahuInteractive.HassleFreeConfig;
using KahuInteractive.VisualFX;
using Unity.Cinemachine;
using UnityEngine;

namespace MarbleBash
{
    
    public class PlayerCameraShakeHandler : MarbleSubComponent
    {
        private CameraShakeConfig _config;
        
        [Header("References:")]
        [SerializeField] private CinemachineImpulseSource _source;

        protected override void Initialise()
        {
            _config = Configuration.Get<CameraShakeConfig>();
        }

        public void DoImpactShakeEvent(float force, Vector3 position)
        {
            // Get the distance lerp, which is how far the cam shake event is within the minimum range
            float distance = Vector3.Distance(Player.transform.position, position);
            float distanceLerp = (distance - _config.minimumRange) / (_config.maximumRange - _config.minimumRange);
            
            // Intensity is multipled by its distance to the player.
            float intensityMultiplier = 1f - Mathf.Clamp(distanceLerp, 0, 1);
            DoImpactShake(force * intensityMultiplier);
        }

        public void DoImpactShake(float force)
        {
            if (force < _config.minimumForce)
            {
                return;
            }

            DoCameraShake(force, _config.minimumForce, _config.maximumForce, _config.impactForceintensityMultiplier);
        }

        public void DoDamageShake(float damage)
        {
            if (damage < _config.maximumDamage)
            {
                return;
            }
            
            DoCameraShake(damage, _config.minimumDamage, _config.maximumDamage, _config.damageIntensityMultiplier);
            
        }



        public void DoCameraShake(float force, float min, float max, float multiplier)
        {

            float lerp = (force - _config.minimumForce) / (_config.maximumForce - _config.minimumForce);

            float impulseForce = _config.intensityCurve.Evaluate(lerp);

            impulseForce *= _config.impactForceintensityMultiplier;
            
            _source.GenerateImpulse(impulseForce);
        }


    }
}