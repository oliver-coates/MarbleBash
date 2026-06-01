using KahuInteractive.VisualFX;
using MarbleBash.Abilities;
using MarbleBash.Enemy;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MarbleBash.VisualEffects
{

    public class VFX_GroundPoundIndicator : VFX_OneShotHandler
    {
        // References:
        [SerializeField] private DecalProjector _projector;

        private const float PROJECTION_DEPTH = 10f;

        private Transform _target;
        private EnemyMovement _targetMovement;
        private Pounding _poundingEffect;

        // private Vector3 _minSize;
        // private Vector3 _maxSize;

        public override void Play(OneShotEffectData data)
        {
            _target = data.transform;

            _targetMovement = data.transform.GetComponentSafe<EnemyMovement>();
            _poundingEffect = data.transform.GetComponentSafe<StatusEffectManager>().GetEffect<Pounding>();
            _poundingEffect.OnFinished += Finish;

            _projector.pivot = new Vector3(0f, 0f, PROJECTION_DEPTH / 2f);
            // _minSize = new Vector3(0.1f, 5f, 0.1f);
            // _maxSize = new Vector3(data.strength, 5f, data.strength);
        }

        private void Update()
        {
            transform.position = _target.transform.position;
            
            float damageDiameter = _poundingEffect.GetDamageRadius() * 2f;
            Vector3 size = new Vector3(damageDiameter, damageDiameter, PROJECTION_DEPTH);

            _projector.size = size; 

            float opacity = 1f - (_targetMovement.distanceToGround / 20f);
            _projector.fadeFactor = opacity;
        }

        public override void Finish()
        {
            _poundingEffect.OnFinished -= Finish;
            Destroy(gameObject);
        }
    }


}

