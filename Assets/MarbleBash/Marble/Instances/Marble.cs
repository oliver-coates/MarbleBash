using UnityEngine;
using MarbleBash.Abilities;

namespace MarbleBash
{
    /// <summary>
    /// Marble class acts as a shared access point for abilities to act upon.
    /// When an ability does a certain action, it is applied against the components
    /// defined in this class. (Example: the dash ability pushes the rigidbody in
    /// the direction of _lookDirection)
    /// </summary>
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


        private Vector3 _cachedVelocity;
        public Vector3 cachedVelocity
        {
            get
            {
                return _cachedVelocity;
            }
        }

        public Vector3 lookDirection
        {
            get
            {
                return GetLookDirection();
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

        protected Abilities.AbilityController _abilities;
        public Abilities.AbilityController abilities
        {
            get
            {
                return _abilities;
            }
        }
    
        protected AbilityEffectManager _statusEffects;
        public AbilityEffectManager statusEffects
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
            _abilities = this.GetComponentSafe<Abilities.AbilityController>();
            _statusEffects = this.GetComponentSafe<AbilityEffectManager>();

            _cachedVelocity = new Vector3();

            // Setup subcomponents:
            foreach (MarbleSubComponent subcomponent in transform.GetComponentsInChildren<MarbleSubComponent>())
            {
                subcomponent.Initialise(this);
            }

            Setup();
        }

        private void FixedUpdate()
        {
            _cachedVelocity = _rigidbody.linearVelocity;
        }

        protected abstract void Setup();

        protected abstract Vector3 GetLookDirection();

        

    }
}
