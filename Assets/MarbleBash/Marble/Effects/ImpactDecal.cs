using System;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MarbleBash
{
    [RequireComponent(typeof(DecalProjector))]
    public class ImpactDecal : MonoBehaviour
    {
        private MovementConfig _config;
        private DecalProjector _projector;
        private float _strength;
        private float _opacity;

        public void Setup(Collision c, float force)
        {
            _config = Configuration.Get<MovementConfig>();
            _projector = this.GetComponentSafe<DecalProjector>();

            _strength = CalculateStrength(force);
            _opacity = _strength;

            SetPosition(c);
            SetSize();
        }

        private void SetSize()
        {
            float size = Mathf.Lerp(_config.impactDecalMinSize, _config.impactDecalMaxSize, _config.impactDecalSizeCurve.Evaluate(_strength));
            _projector.size = new Vector3(size, size, size);
        }

        private float CalculateStrength(float force)
        {
            float strengthL = (force - _config.minimumVelocityRequiredForImpactDecal) / (_config.maxVelocityForImpactDecal - _config.minimumVelocityRequiredForImpactDecal);

            return _config.impactStartOpacityCurve.Evaluate(strengthL);
        }

        private void SetPosition(Collision c)
        {
            ContactPoint hit = c.GetContact(0);
            
            Vector3 pos = hit.point + hit.normal;
            Quaternion rot = Quaternion.LookRotation(-hit.normal);

            transform.SetPositionAndRotation(pos, rot);

            transform.Rotate(0f, 0f, UnityEngine.Random.Range(0, 360f));
        }

        private void Update()
        {
            _opacity -= Time.deltaTime * 0.1f;

            _projector.fadeFactor = _opacity;

            if (_opacity < 0f)
            {
                Destroy(gameObject);
            }
        }
    }


}

