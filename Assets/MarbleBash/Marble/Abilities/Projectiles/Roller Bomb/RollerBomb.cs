using System;
using KahuInteractive.HassleFreeConfig;
using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.Abilities
{
    public class RollerBomb : MonoBehaviour
    {
        #region Configuration        
        private const float TIME_TO_ACTIVATE = 1f;
        
        private float _rollSpeed;
        private float _duration;
        private float _explodeNearTargetRadius;
        private float _deactivateNearTargetRadius;
        private float _explosionRadius;
        private float _explosionDamage;
        #endregion

        // References:
        [SerializeField] private GameObject _explosionPrefab;
        private Rigidbody _rb;

        // State:
        private Marble _caster;
        private Marble _target;
        private float _timeAlive;
        private bool _hasActivated;
        private bool _hasDeactivated;

        public void Initialise(Marble caster, Marble targetMarble)
        {
            _rb = this.GetComponentSafe<Rigidbody>();
            
            _target = targetMarble;
            _caster = caster;

            GetConfigValues();

            _timeAlive = 0f;
            _hasActivated = false;
        
            gameObject.layer = LayerMask.NameToLayer("Debris");

            Throw();
        }

        private void GetConfigValues()
        {
            _rollSpeed = 250f;
            _duration = 5f;
            _explodeNearTargetRadius = 0.75f;
            _deactivateNearTargetRadius = 2f;
        
            _explosionRadius = 1f;
            _explosionDamage = 5f;
        }


        private void Throw()
        {
            Vector2 random2d = UnityEngine.Random.onUnitCircle;
            float randomUp = UnityEngine.Random.Range(1.5f, 3f);

            Vector3 throwDir = new Vector3(random2d.x, randomUp, random2d.y).normalized;
            float throwForce = 50 * UnityEngine.Random.Range(0.75f, 1.25f);

            _rb.AddForce(throwDir * throwForce);
        }

        private void Update()
        {
            _timeAlive += Time.deltaTime;

            if (_hasActivated)
            {
                if (!_hasDeactivated)
                {
                    ChaseTarget();     
                    if (CheckDistanceToTargetLessThan(_deactivateNearTargetRadius))
                    {
                        _hasDeactivated = true;
                    }           
                }

                if (_timeAlive > _duration || CheckDistanceToTargetLessThan(_explodeNearTargetRadius))
                {
                    Explode();
                }
            }
            else
            {
                if (_timeAlive > TIME_TO_ACTIVATE)
                {
                    Activate();
                }
            }
            
        }

        private void ChaseTarget()
        {
            Vector3 dirToTarget = (_target.transform.position - transform.position).normalized;

            _rb.AddForce(dirToTarget * _rollSpeed * Time.deltaTime);
        }

        private void Activate()
        {
            _hasActivated = true;
            gameObject.layer = LayerMask.NameToLayer("Default");           
        }
    
        private bool CheckDistanceToTargetLessThan(float than)
        {
            return Vector3.Distance(transform.position, _target.transform.position) < than;
        }

        private void Explode()
        {
            BombExplosion explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity).GetComponent<BombExplosion>();
            explosion.Initialise(_explosionRadius, _explosionDamage, false, _caster);

            OneShotEffectData data = new OneShotEffectData("Ground Pound", transform.position, Quaternion.identity, _explosionRadius);
            VFX.Play(data);

            Destroy(gameObject);
        }
    }
}
