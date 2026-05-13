using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.VisualEffects
{

    public class VFX_DazedParticles : VFX_OneShotHandler
    {
        [SerializeField] private ParticleSystem _particles;
        private Marble _marble;
        private float _timer;

        public override void Play(OneShotEffectData data)
        {
            _marble = data.transform.GetComponentSafe<Marble>();            
            
            _timer = data.strength;

            float marbleRadius = _marble.transform.localScale.x  / 2f;
            _particles.transform.localPosition = new Vector3(0f, marbleRadius*1.2f, 0f);
            
            // Setup particles:
            var shape = _particles.shape;
            shape.radius = marbleRadius*1.2f;

            var main = _particles.main;
            main.startLifetime = _timer;

            _particles.Emit(4 + (int)(marbleRadius / 0.25f));
        }

        private void Update()
        {
            transform.position = _marble.transform.position;

            _timer -= Time.deltaTime;
            if (_timer < 0f)
            {
                Destroy(gameObject);
            }
        }
    }

}
