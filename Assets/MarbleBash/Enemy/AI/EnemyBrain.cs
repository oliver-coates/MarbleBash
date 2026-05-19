using UnityEngine;

namespace MarbleBash.Enemy
{
    public class EnemyBrain : MarbleSubComponent
    {
        private EnemyMovement _movement;
        internal EnemyMovement movement
        {
            get
            {
                return _movement;
            }
        }

        private Tactic _currentTactic;

        internal Marble marble
        {
            get
            {
                return _marble;
            }
        }

        protected override void Initialise()
        {
            _movement = (EnemyMovement) _marble.movement;
            StartTactic<Attack>();
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