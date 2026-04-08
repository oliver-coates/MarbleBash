using UnityEngine;

namespace MarbleBash.Abilities
{

    [System.Serializable]
    public abstract class Ability
    {
        /// <summary>
        /// The 'Subject' who has/is casting this ability.
        /// </summary>
        protected Marble _subject;

        [Header("Decorator:")]
        [SerializeField] protected string _name;
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

        [Header("Enemy AI use requriements:")]
        [SerializeField] private EnemyAbilityUseRequirement[] _useRequirements;

        public Ability(Marble subject)
        {
            _subject = subject;
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


        public void SetupEnemyUseRequirements()
        {
            _useRequirements = GetEnemyUseRequirements();
        }

        /// <summary>
        /// Gets a list of Ability Use Requirements which dictate whether an enemy AI should use this ability or not.
        /// For example: The fireball ability requires a line of sight to the player.
        /// </summary>
        protected abstract EnemyAbilityUseRequirement[] GetEnemyUseRequirements();

        /// <summary>
        /// Loops through all Enemy Ability Use Requirements and ensures they are all true.
        /// Returns true if all use requirements are met.
        /// </summary>
        public bool EvaluateEnemyUseRequirements()
        {
            foreach (EnemyAbilityUseRequirement requirement in _useRequirements)
            {
                if (requirement.Evaluate() == false)
                {
                    return false;
                }
            }

            return true;
        }

        internal void Tick()
        {
            _cooldownTimer -= Time.deltaTime;        
        }        
    }


}
