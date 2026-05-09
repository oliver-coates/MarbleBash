using KahuInteractive.HassleFreeConfig;
using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.VisualEffects
{

    public class VFX_DynamicParticleEmissionRate : VFX_ContinuousEffectHandler
    {
        private float _emissionTimer;
        private MovementConfig _c;
        private ParticleSystem _particles;

        protected override void Start()
        {
            base.Start();

            _c = Configuration.Get<MovementConfig>();
            _particles = this.GetComponentSafe<ParticleSystem>();
        }

        private void Update()
        {
            // Set renderer:
            // _particles.time = _c.trailTimeCurve.Evaluate(_emissionIntensity);
            // _particles.startWidth = _c.trailWidthCurve.Evaluate(_emissionIntensity);
            // _particles.SetParticles()

            float emissionRate = 0;
            if (_value > _c.minimumVelocityForTrail)
            {
                emissionRate = (_value - _c.minimumVelocityForTrail) / (_c.maxVelocityForTrail - _c.minimumVelocityForTrail);            
            }

            _emissionTimer += emissionRate * _c.trailIntensityMultiplier;
            
            if (_emissionTimer > 1.0f)
            {
                _emissionTimer -= 1.0f;
                _particles.Emit(1);
            }

        }
    }


}
