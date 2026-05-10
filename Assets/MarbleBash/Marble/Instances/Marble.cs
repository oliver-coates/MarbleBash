using UnityEngine;
using MarbleBash.Abilities;

namespace MarbleBash
{
    [System.Serializable]
    public abstract class Marble : MonoBehaviour
    {
        [Header("Stats:")]
        [SerializeField] protected MarbleStats _stats;
        public MarbleStats stats
        {
            get
            {
                return _stats;
            }
        }

        [Header("Health:")]
        [SerializeField] protected MarbleHealth _health;
        public MarbleHealth health
        {
            get
            {
                return _health;
            }
        }

        [Header("Movement:")]
        [SerializeField] private MarbleMovement _movement;
        public MarbleMovement movement
        {
            get
            {
                return _movement;
            }	
        }

        protected Rigidbody _rigidbody;
        public new Rigidbody rigidbody
        {
            get
            {
                return _rigidbody;
            }
        }

        protected AbilityController _abilities;
        public AbilityController abilities
        {
            get
            {
                return _abilities;
            }
        }
    
        protected AbilityEffectManager _statusEffects;
        public AbilityEffectManager abilityEffects
        {
            get
            {
                return _statusEffects;
            }
        }

        

        internal void Initialise()
        {
            _health = new MarbleHealth(this);
            
            _rigidbody = this.GetComponentSafe<Rigidbody>();
            _abilities = this.GetComponentSafe<AbilityController>();
            _statusEffects = this.GetComponentSafe<AbilityEffectManager>();
            _movement = this.GetComponentSafe<MarbleMovement>();

            // Setup subcomponents:
            foreach (MarbleSubComponent subcomponent in transform.GetComponentsInChildren<MarbleSubComponent>())
            {
                subcomponent.Initialise(this);
            }

            Setup();
        }

        protected abstract void Setup();

    }
}
