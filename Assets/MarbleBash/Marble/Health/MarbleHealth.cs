using System;
using System.Linq.Expressions;
using UnityEngine;

namespace MarbleBash
{

    [System.Serializable]
    public class MarbleHealth
    {
        private Marble _marble;

        /// <summary>
        /// Called when number of lives has changed,
        /// int - The num of lives remaining
        /// </summary>
        public event Action<int> OnLivesChanged;

        public event Action OnDied;

        /// <summary>
        /// Called when taken damage from any source.
        /// HealthChangedEvent contains all the information regarding the change in health.
        /// </summary>
        public event Action<HealthChangedEvent> OnDamageTaken;
        public static event Action<HealthChangedEvent> OnDamageTakenGlobal; 

        [Header("Lives:")]
        [SerializeField] private int _lives;
        public int lives
        {
            get
            {
                return _lives;
            }	
        }    
    
        [SerializeField] private bool _isDead;
        public bool isDead
        {
            get
            {
                return _isDead;
            }	
        }


        [Header("Health:")]
        [SerializeField] private float _hp;
        public float health
        {
            get
            {
                return _hp;
            }	
        }
    
        [SerializeField] private float _maxHealth;
        public float maxHealth
        {
            get
            {
                return _maxHealth;
            }	
        }

        [Header("Shield:")]
        [SerializeField] private float _shield;
        public float shield
        {
            get
            {
                return _shield;
            }	
        }
    
        [SerializeField] private float _maxShield;
        public float maxShield
        {
            get
            {
                return _maxShield;
            }	
        }


        public MarbleHealth(Marble marble)
        {
            _marble = marble;

            _lives = 1;

            _maxHealth = 25f;
            _hp = 25f;

            _maxShield = 0;
            _shield = 0;
        }

        public void TakeDamage(DamageEvent damageEvent)
        {
            HealthChangedEvent newEvent = new (damageEvent);

            // Apply shield:
            if (_shield >= 0)
            {
                _shield -= damageEvent.amount;            
                newEvent.shieldChange = Mathf.Clamp(damageEvent.amount, 0, 100000000);
            }

            // Check to see if shield is broken, if so, apply overflowing damage to health
            if (_shield < 0)
            {
                float healthChange = Mathf.Abs(_shield);
                _hp -= healthChange;
                _shield = 0;

                newEvent.shieldBroken = true;
                newEvent.healthChange = healthChange;

                // Check to see if our health has fallen below 0
                if (_hp < 0)
                {
                    LoseLife();
                }
            }

            // Apply knockback:
            Vector3 knockbackForce = damageEvent.knockbackAmount * damageEvent.knockbackDirection.normalized;
            _marble.rigidbody.AddForce(knockbackForce, ForceMode.Impulse);
        
            // Finish by firing event
            OnDamageTaken?.Invoke(newEvent);
            OnDamageTakenGlobal?.Invoke(newEvent);
        }

        private void LoseLife()
        {
            _lives -= 1;
            _hp = _maxHealth;
            _shield = _maxShield;

            if (_lives == 0)
            {
                Die();
            }

            OnLivesChanged?.Invoke(_lives);
        }

        public void FellOffMap()
        {
            DamageEvent fallDamage = new DamageEvent(null, _marble)
            {
                amount = 100000,
                knockbackAmount = 0,
                knockbackDirection = Vector3.zero,
                doDamageEffects = false
            };

            TakeDamage(fallDamage);
        }

        private void Die()
        {
            _isDead = true;
            OnDied?.Invoke();
        }

        public class HealthChangedEvent
        {
            public HealthChangedEvent(DamageEvent damageEvent)
            {
                this.marble = damageEvent.target;
                this.damage = damageEvent;
                this.totalHealthChange = -damageEvent.amount;
            }

            public Marble marble;
            public DamageEvent damage;
            public float totalHealthChange;
            public float healthChange;
            public float shieldChange;
            public bool shieldBroken;
        }
    }


}