using UnityEngine;

namespace MarbleBash.Enemy
{
    public class EnemyBrain : MarbleSubComponent
    {
        #region References
        private EnemyMovement _movement;
        internal EnemyMovement movement
        {
            get
            {
                return _movement;
            }
        }
        internal Marble marble
        {
            get
            {
                return _marble;
            }
        }
        #endregion

        #region Tactics
        private Tactic _currentTactic;
        [SerializeField] private Tactic[] _tactics;

        #endregion


        // Note this Initialise method, as part of the MarbleSubComoponent, is called but not used.
        // Instead the other Initialise method is called by the EnemyBrainInitialiser to prime this class with its tactics
        protected override void Initialise()
        {
        }

        internal void Initialise(Tactic[] tactics)
        {
            _movement = (EnemyMovement) _marble.movement;
            
            _tactics = tactics;
            _currentTactic = tactics[0];
        }

        private void Update()
        {
            _currentTactic.Tick();
        }

        internal void StartTactic<T>() where T : Tactic, new()
        {
            T newTactic = new ();
            newTactic.Initialise(this);
            _currentTactic = newTactic;
        }

        internal void FlowOnToNextTactic(Tactic newTactic)
        {
            _currentTactic.Finish(Tactic.FinishReason.DurationExpired);

            newTactic.Initialise(this);
            _currentTactic = newTactic;
        }

        internal void TransitionToTactic(Tactic newTactic)
        {
            _currentTactic.Finish(Tactic.FinishReason.TransitionOccured);

            newTactic.Initialise(this);
            _currentTactic = newTactic;
        }

        internal void SetPathTarget(Vector3 position)
        {
            _movement.SetPathingTarget(position);            
        } 


        
    }
}