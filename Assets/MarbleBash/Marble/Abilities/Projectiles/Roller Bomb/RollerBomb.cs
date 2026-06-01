using System;
using KahuInteractive.HassleFreeConfig;
using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash.Abilities
{
    public class RollerBomb : MonoBehaviour
    {
        private enum State
        {
            Asleep = 0,
            Chasing = 1,
            Activated = 2,
            Exploding = 3
        }

        #region Configuration        
        private const float TIME_TO_CHASE = 1f;
        private const float IMPULSE_TO_MAKE_EXPLODE = 0.5f;

        private float _rollSpeed;

        private float _destroyAfterTime;
        
        private float _activateNearTargetRadius;
        private float _explodeOnceActivatedTime;
        private float _explodeTime;

        private float _explosionRadius;
        private float _explosionDamage;
        #endregion

        // References:
        [SerializeField] private GameObject _explosionPrefab;
        private Rigidbody _rb;

        // State:
        private State _state;
        private float _stateTimer;

        private float _lifeTimer;

        private Marble _caster;
        private Marble _target;
        private Vector3 _impactNormal;

        public void Initialise(Marble caster, Marble targetMarble)
        {
            _rb = this.GetComponentSafe<Rigidbody>();
            
            _target = targetMarble;
            _caster = caster;

            GetConfigValues();

            _stateTimer = 0f;
            _impactNormal = Vector3.up;

            ChangeStateTo(State.Asleep);

            Throw();
        }

        private void GetConfigValues()
        {
            _rollSpeed = 250f;
            _destroyAfterTime = 5f;
            
            _explodeOnceActivatedTime = 2f;
            _activateNearTargetRadius = 2f;
            _explodeTime = 0.125f;
        
            _explosionRadius = 1f;
            _explosionDamage = 5f;
        }


        private void Throw()
        {
            Vector2 random2d = UnityEngine.Random.onUnitCircle;
            float randomUp = UnityEngine.Random.Range(1.5f, 3f);

            Vector3 throwDir = new Vector3(random2d.x, randomUp, random2d.y).normalized;
            float throwForce = 75 * UnityEngine.Random.Range(0.75f, 1.25f);

            _rb.AddForce(throwDir * throwForce);
        }

        private void Update()
        {
            _stateTimer += Time.deltaTime;
            _lifeTimer += Time.deltaTime;

            switch (_state)
            {
                case State.Asleep:
                    if (_stateTimer > TIME_TO_CHASE)
                    {
                        ChangeStateTo(State.Chasing);
                    }
                    break;
                
                case State.Chasing:
                    ChaseTarget();
                    if (Vector3.Distance(transform.position, _target.transform.position) < _activateNearTargetRadius)
                    {
                        ChangeStateTo(State.Activated);
                    }
                    break;
                
                case State.Activated:
                    if (_stateTimer > _explodeOnceActivatedTime)
                    {
                        ChangeStateTo(State.Exploding);
                    }
                    break;
                
                case State.Exploding:
                    if (_stateTimer > _explodeTime)
                    {
                        Explode(_impactNormal);
                    }
                    break;
            }
            
            if (_state != State.Exploding && _lifeTimer > _destroyAfterTime)
            {
                ChangeStateTo(State.Exploding);
            }
        }

        private void ChaseTarget()
        {
            Vector3 dirToTarget = (_target.transform.position - transform.position).normalized;

            _rb.AddForce(dirToTarget * _rollSpeed * Time.deltaTime);
        }

        private void ChangeStateTo(State newState)
        {
            _state = newState;
            _stateTimer = 0f;

            switch (newState)
            {
                case State.Asleep:
                    gameObject.layer = LayerMask.NameToLayer("Debris");
                    return;

                case State.Chasing:
                    gameObject.layer = LayerMask.NameToLayer("Default");
                    return;

                case State.Activated:
                    return;

                case State.Exploding:
                    return;   
            }
        }

        private void OnCollisionEnter(Collision c)
        {
            if (_state != State.Activated)
            {
                return;
            }

            if (c.impulse.magnitude > IMPULSE_TO_MAKE_EXPLODE)
            {
                _impactNormal = c.contacts[0].normal;
                ChangeStateTo(State.Exploding);
            }
        }

        private void Explode(Vector3 normal)
        {
            BombExplosion explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity).GetComponent<BombExplosion>();
            explosion.Initialise(transform.position, normal, _explosionRadius, _explosionDamage, false, _caster);

            Destroy(gameObject);
        }
    }
}
