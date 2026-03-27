using UnityEngine;

namespace MarbleBash.Abilities
{

    [System.Serializable]
    public abstract class Ability
    {

        [Header("Decorator:")]
        [SerializeField] internal string _name;
        public string name
        {
            get
            {
                return _name;
            }
        }

        [Header("State:")]
        [SerializeField] private bool _hasCooldown;
        [SerializeField] private float _cooldownTimer;

        public Ability()
        {
            // Default values, repalce with some kind of constructor parameter:
            _hasCooldown = true;
            _cooldownTimer = 2f;
        }

        /// <summary>
        /// Attempts to activate this ability.
        /// </summary>
        /// <returns>True if it was able to activate</returns>
        internal bool AttemptActivate()
        {            
            if (CheckIsNotOnCooldown() && IsAbleToActivate())
            {
                Activate();
                _cooldownTimer = 2f;
                return true;
            } 
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Activates this ability
        /// </summary>
        protected abstract void Activate(); 

        /// <summary>
        /// Checks that this ability is avaialble to be activated.
        /// </summary>
        /// <returns>True if able to active.</returns>
        internal bool CheckIsNotOnCooldown()
        {
            if (_hasCooldown)
            {
                return (_cooldownTimer <= 0);
            }
            else
            {
                return _hasCooldown;
            }
        }
        
        /// <summary>
        /// Checks if this ability is able to activate.
        /// This is the internal method, implement in child classes.
        /// </summary>
        /// <returns> True if able to activate.</returns>
        protected abstract bool IsAbleToActivate();


        internal void Tick()
        {
            _cooldownTimer -= Time.deltaTime;        
        }        
    }


}
