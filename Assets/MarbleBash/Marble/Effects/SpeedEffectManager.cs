using System;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    public class SpeedEffectManager : MonoBehaviour
    {
        private MovementConfig _c;
        private ParticleSystem _particles;
        [SerializeField] private float _targetIntensity;
        [SerializeField] private float _emissionIntensity;

        private float _emissionTimer;

        private void Start()
        {
            _c = Configuration.Get<MovementConfig>();
            _particles = this.GetComponentSafe<ParticleSystem>();
        }


        private void Update()
        {
            // Find the emission intensity from the player's velocity 
            _targetIntensity = Mathf.Clamp((Player.rigidbody.linearVelocity.magnitude - _c.minimumVelocityForTrail) / _c.maxVelocityForTrail, 0, 1);

            // Actual intensity smoothly lerps towards it
            _emissionIntensity = Mathf.Lerp(_emissionIntensity, _targetIntensity, Time.deltaTime * _c.trailIntensityChangeTightness);

            // Set renderer:
            // _particles.time = _c.trailTimeCurve.Evaluate(_emissionIntensity);
            // _particles.startWidth = _c.trailWidthCurve.Evaluate(_emissionIntensity);
            // _particles.SetParticles()

            _emissionTimer += _emissionIntensity;
            if (_emissionTimer > 1.0f)
            {
                _emissionTimer -= 1.0f;
                _particles.Emit(1);
            }

        }
    }


}

