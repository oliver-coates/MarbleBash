using System;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    public class TrailRendererManager : MonoBehaviour
    {
        private MovementConfig _c;
        private TrailRenderer _renderer;
        [SerializeField] private float _targetIntensity;
        [SerializeField] private float _emissionIntensity;

        private void Start()
        {
            _c = Configuration.Get<MovementConfig>();
            _renderer = this.GetComponentSafe<TrailRenderer>();
        }


        private void Update()
        {
            // Find the emission intensity from the player's velocity 
            _targetIntensity = Mathf.Clamp((Player.rigidbody.linearVelocity.magnitude - _c.minimumVelocityForTrail) / _c.maxVelocityForTrail, 0, 1);

            // Actual intensity smoothly lerps towards it
            _emissionIntensity = Mathf.Lerp(_emissionIntensity, _targetIntensity, Time.deltaTime * _c.trailIntensityChangeTightness);

            // Set renderer:
            _renderer.time = _c.trailTimeCurve.Evaluate(_emissionIntensity);
            _renderer.startWidth = _c.trailWidthCurve.Evaluate(_emissionIntensity);

        }
    }


}

