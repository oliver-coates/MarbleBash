using UnityEngine;

namespace MarbleBash
{

    public class GroundPoundEffectManager : MonoBehaviour, IVisualEffectManager
    {
        private ParticleSystem _particles;

        private void Awake()
        {
            _particles = this.GetComponentSafe<ParticleSystem>();
        }

        public void CreateEffect(Vector3 position, float strength)
        {
            transform.position = position;

            var shape = _particles.shape;
            shape.radius = strength / 2f;

            _particles.Play();
        }
    }


}

