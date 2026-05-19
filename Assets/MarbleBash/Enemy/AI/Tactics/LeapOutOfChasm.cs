using MarbleBash.Enemy;
using UnityEditorInternal;
using UnityEngine;

namespace MarbleBash.Enemy
{

    internal class LeapOutOfChasm : Tactic
    {
        /// <summary>
        /// The position that the marble tries to leap towards.
        /// </summary>
        private Vector3 leapTarget;

        protected override void Start()
        {
            _duration = 2f;

            TacticTransition backOnGround = new(
                _brain, 
                typeof(Attack),
                new TransitionCriteria[] { new IsGrounded(_brain) }
            );
            _transtions.Add(backOnGround);
        
            leapTarget = _brain.marble.movement.groundedPosition + Vector3.up;

            _brain.movement.MoveTowardsPoint(leapTarget);
        }



        protected override void Update()
        {
        }

        protected override void OnTransition()
        {
            _brain.movement.MoveAlongPath();
        }

        protected override void OnDurationFinished()
        {
        }

        protected override Tactic GetNextTactic()
        {
            return new LeapOutOfChasm();
        }
    }
}