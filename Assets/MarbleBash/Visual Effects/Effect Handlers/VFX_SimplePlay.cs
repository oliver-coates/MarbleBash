using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.VisualEffects
{

    public class VFX_SimplePlay : VisualEffectHandler
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

            _particles.Play();
        }
    }


}

