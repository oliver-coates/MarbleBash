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

        protected MarbleCollisionHandler _collisionHandler;
        public MarbleCollisionHandler collisionHandler
        {
            get
            {
                return _collisionHandler;
            }
        }

        protected MarbleMaterialAcessor _materials;
        public MarbleMaterialAcessor materials
        {
            get
            {
                return _materials;
            }
        }

        protected bool _isPlayer;
        public bool isPlayer
        {
            get
            {
                return _isPlayer;
            }
        }

        internal void Initialise()
        {
            _health = new MarbleHealth(this);
            
            _rigidbody = this.GetComponentSafe<Rigidbody>();
            _abilities = this.GetComponentSafe<AbilityController>();
            _statusEffects = this.GetComponentSafe<AbilityEffectManager>();
            _movement = this.GetComponentSafe<MarbleMovement>();
            _collisionHandler = this.GetComponentSafe<MarbleCollisionHandler>();
            _materials = this.GetComponentInChildren<MarbleMaterialAcessor>();

            _isPlayer = this == Player.instance;

            // Setup subcomponents:
            foreach (MarbleSubComponent subcomponent in transform.GetComponentsInChildren<MarbleSubComponent>())
            {
                subcomponent.Initialise(this);
            }

            Setup();

            _stats.marbleSize.OnChange += UpdateSize;
            _stats.rigidbodyMass.OnChange += UpdateMass;
            _stats.rigidbodyDrag.OnChange += UpdateDrag;

            _stats.RecalulcateAllStats();
        }   

        protected abstract void Setup();

        private void UpdateSize(float size)
        {
            transform.localScale = new Vector3(size, size, size);
        }

        private void UpdateMass(float mass)
        {
            _rigidbody.mass = mass;
        }

        private void UpdateDrag(float drag)
        {
            _rigidbody.linearDamping = drag;
            _rigidbody.angularDamping = drag / 2f;
        }

    }
}
