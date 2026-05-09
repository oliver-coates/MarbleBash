using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.VisualEffects
{
    public class VFX_SizeAdjustedPlay : VFX_OneShotHandler
    {
        private ParticleSystem _particles;

        private void Awake()
        {
            _particles = this.GetComponentSafe<ParticleSystem>();
        }


        
        public override void Play(OneShotEffectData data)
        {
            transform.position = data.position;
            transform.rotation = Quaternion.identity;

            var shape = _particles.shape;
            shape.radius = data.strength / 2f;

            int emitCount = Mathf.CeilToInt(data.strength) * 5;

            _particles.Emit(emitCount);            
        }
    }
}
