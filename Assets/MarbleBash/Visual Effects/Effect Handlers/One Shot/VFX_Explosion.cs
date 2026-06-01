using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.VisualEffects
{

    public class VFX_Explosion : VFX_OneShotHandler
    {
        // References:
        [SerializeField] private ParticleSystem _smokeParticles;
        [SerializeField] private ParticleSystem _shardsParticles;


        public override void Play(OneShotEffectData data)
        {
            float radius = data.strength;
            transform.position = data.position;
            transform.rotation = data.rotation;

            int numShards = 3 + Mathf.CeilToInt(radius / 10f);
            _shardsParticles.Emit(numShards);

            var smokeShape = _smokeParticles.shape;
            smokeShape.radius = radius;

            int numSmoke = Mathf.CeilToInt(radius * 30f);
            _smokeParticles.Emit(numSmoke);

        }

        public override void Finish()
        {
            Destroy(gameObject, 1f);
        }
    }


}

