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
        private Vector3 _leapTarget;
        private bool _hasLeaped;

        protected override void Start()
        {
            _hasLeaped = false;
            _duration = 5f;

            TacticTransition backOnGround = new(
                _brain, 
                typeof(Attack),
                new TransitionCriteria[] { new IsGrounded(_brain) }
            );
            _transtions.Add(backOnGround);

            _leapTarget = GetLeapTarget();

            _brain.movement.MoveTowardsPoint(_leapTarget);
        }

        protected override void Update()
        {
            if (_timeSinceStart > 0.25 && !_hasLeaped)
            {
                if (_marble.abilities.IsAbilityAbleToActivate("Dash"))
                {
                    _marble.rigidbody.linearVelocity *= 0.5f;
                    _hasLeaped = _marble.abilities.AttemptActivateAbility("Dash");
                }
            }
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
        
        private Vector3 GetLeapTarget()
        {
            Vector3 groundedPosition = _marble.movement.groundedPosition;

            Vector3 dirToGroundedPosition = (groundedPosition.ShearTo2D() - _marble.transform.position.ShearTo2D()).normalized;
            
            
            return groundedPosition + (Vector3.up * 2f) + (dirToGroundedPosition * 2f);
        }
    }
}