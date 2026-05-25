using UnityEngine;
using MarbleBash.Abilities;
using MarbleBash.Upgrades;

namespace MarbleBash
{
    [System.Serializable]
    public abstract class Marble : MonoBehaviour
    {

        #region Core Subcomponents:
        protected MarbleLevel _level;
        public MarbleLevel level => _level;

        private MarbleHealth _health;
        public MarbleHealth health => _health;

        private MarbleMovement _movement;
        public MarbleMovement movement => _movement;
        
        private MarbleStats _stats;
        public MarbleStats stats => _stats;

        private MarbleUpgrades _upgrades;
        public MarbleUpgrades upgrades => _upgrades;

        #endregion


        #region Additional Subcomponents
        protected AbilityController _abilities;
        public AbilityController abilities
        {
            get
            {
                return _abilities;
            }
        }
    
        protected StatusEffectManager _statusEffects;
        public StatusEffectManager abilityEffects
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
        #endregion


        #region Unity component references
        private Rigidbody _rigidbody;
        public new Rigidbody rigidbody
        {
            get
            {
                return _rigidbody;
            }
        }

        #endregion


        protected bool _isPlayer;
        public bool isPlayer
        {
            get
            {
                return _isPlayer;
            }
        }

        protected void InitialiseInternal()
        {
            _isPlayer = this == Player.instance;

            _health = new MarbleHealth(this);
            
            GetAllSubcomponents();

            // Setup subcomponents:
            foreach (MarbleSubComponent subcomponent in transform.GetComponentsInChildren<MarbleSubComponent>())
            {
                subcomponent.Initialise(this);
            }

            _stats.marbleSize.OnChange += UpdateSize;
            _stats.rigidbodyMass.OnChange += UpdateMass;
            _stats.rigidbodyDrag.OnChange += UpdateDrag;
            _stats.RecalulcateAllStats();
        }   

        private void GetAllSubcomponents()
        {
            _rigidbody = this.GetComponentSafe<Rigidbody>();
            _abilities = this.GetComponentSafe<AbilityController>();
            _statusEffects = this.GetComponentSafe<StatusEffectManager>();
            _movement = this.GetComponentSafe<MarbleMovement>();
            _collisionHandler = this.GetComponentSafe<MarbleCollisionHandler>();
            
            _materials = this.GetComponentInChildrenSafe<MarbleMaterialAcessor>();
            _upgrades = this.GetComponentInChildrenSafe<MarbleUpgrades>();
            _stats = this.GetComponentInChildrenSafe<MarbleStats>();
        }


        #region Rigidbody setting update methods
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
        #endregion
    }
}
