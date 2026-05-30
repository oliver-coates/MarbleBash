using System;
using KahuInteractive.HassleFreeConfig;
using KahuInteractive.VisualFX;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MarbleBash.VisualEffects
{
    [RequireComponent(typeof(DecalProjector))]
    public class ImpactDecal : VFX_OneShotHandler
    {
        private DecalProjector _projector;
        [SerializeField] private ParticleSystem _dustParticles;
        private float _opacity;


        public override void Play(OneShotEffectData data)
        {
            _projector = this.GetComponentSafe<DecalProjector>();

            transform.SetPositionAndRotation(data.position, data.rotation);
            transform.Rotate(0f, 0f, UnityEngine.Random.Range(0, 360f));

            float size = data.strength;
            _projector.size = new Vector3(size, size, 0.1f);

            _opacity = 1f;
            PlayParticles(size);
        }

        private void PlayParticles(float size)
        {   
            // Dust particles:
            var shape = _dustParticles.shape;
            shape.radius = size / 2f;

            var emission = _dustParticles.emission;
            var burst = emission.GetBurst(0);
            burst.count = size * 20;
            emission.SetBurst(0, burst);

            // Rocks particles:
            // ... 

            // Finally play:
            _dustParticles.Play();
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

        public override void Finish()
        {
            
        }



    }


}

