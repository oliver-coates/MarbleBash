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
        /// int - The change in number of lives
        /// </summary>
        public event Action<int> OnLivesChanged;

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
    
        [Header("Health:")]
        [SerializeField] private float _health;
        public float health
        {
            get
            {
                return _health;
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

            _lives = 3;

            _maxHealth = 100f;
            _health = 100f;

            _maxShield = 0;
            _shield = 0;
        }

        public void TakeDamage(DamageEvent damageEvent)
        {
            HealthChangedEvent newEvent = new (damageEvent.target);
            newEvent.totalHealthChange = -damageEvent.amount;

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
                _health -= healthChange;
                _shield = 0;

                newEvent.shieldBroken = true;
                newEvent.healthChange = healthChange;

                // Check to see if our health has fallen below 0
                if (_health < 0)
                {
                    LoseLife();
                }
            }

            // Apply knockback:
            Vector3 knockbackForce = damageEvent.knockbackAmount * (-damageEvent.direction + (Vector3.up * 0.33f));
            _marble.rigidbody.AddForce(knockbackForce, ForceMode.Impulse);
        
            // Finish by firing event
            OnDamageTaken?.Invoke(newEvent);
            OnDamageTakenGlobal?.Invoke(newEvent);
        }

        private void LoseLife()
        {
            _lives -= 1;
            _health = _maxHealth;
            _shield = _maxShield;

            OnLivesChanged?.Invoke(-1);
        }

        public class HealthChangedEvent
        {
            public HealthChangedEvent(Marble marble)
            {
                this.marble = marble;
            }

            public Marble marble;
            public float totalHealthChange;
            public float healthChange;
            public float shieldChange;
            public bool shieldBroken;
        }
    }


}