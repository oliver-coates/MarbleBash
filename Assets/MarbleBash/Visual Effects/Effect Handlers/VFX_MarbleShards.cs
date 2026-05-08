using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.VisualEffects
{

    public class VFX_MarbleShards : VisualEffectHandler
    {
        private ParticleSystem _particles;

        private void Awake()
        {
            _particles = this.GetComponentSafe<ParticleSystem>();
        }

        public override void Play(OneShotEffectData data)
        {
            transform.position = data.position;
            transform.rotation = data.rotation;

            int emitCount = Random.Range(0, 4) + Mathf.CeilToInt(-data.strength / 10);;

            _particles.Emit(emitCount);
        }
    }


}

