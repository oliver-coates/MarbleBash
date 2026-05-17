using UnityEngine;

namespace MarbleBash.Enemy
{
    public class EnemyBrain : MarbleSubComponent
    {
        private EnemyMovement _movement;

        private Tactic _currentTactic;

        protected override void Initialise()
        {
            _movement = (EnemyMovement) _marble.movement;
        }

        private void Update()
        {
            _currentTactic.Tick();
        }

        internal void ChangeTactic<T>() where T : Tactic, new()
        {
            T newTactic = new ();

            newTactic.Initialise(this);

            _currentTactic = newTactic;
        }

        internal void SetPathTarget(Vector3 position)
        {
            _movement.SetPathingTarget(position);            
        } 


        
    }
}