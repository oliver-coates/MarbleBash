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
        private TacticTransition _overChasmTransition;

        protected override void Start()
        {
            _duration = 5f;

            _hasLeaped = false;
            _overChasmTransition.enabled = false;

            _leapTarget = GetLeapTarget();

            _brain.movement.MoveTowardsPoint(_leapTarget);
        }

        internal override void SetupTransitions()
        {
            // Transition to this state when we are over a chasm
            _overChasmTransition = new TacticTransition (
                this,
                new TransitionCriteria[]
                {
                    _brain.AddCriteria<IsOverChasm>()
                }
            );
            _brain.AddGeneralTransition(_overChasmTransition);

            // Transition back to the default state when we are back on the ground
            TacticTransition backOnGround = new(
                _brain.defaultTactic,
                new TransitionCriteria[] { _brain.AddCriteria<IsGrounded>() }
            );
            _transtions.Add(backOnGround);
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
            _overChasmTransition.enabled = true;
        }

        protected override void OnDurationFinished()
        {
        }

        protected override Tactic GetNextTactic()
        {
            return this;
        }
        
        private Vector3 GetLeapTarget()
        {
            Vector3 groundedPosition = _marble.movement.groundedPosition;

            Vector3 dirToGroundedPosition = (groundedPosition.ShearTo2D() - _marble.transform.position.ShearTo2D()).normalized;
            
            
            return groundedPosition + (Vector3.up * 2f) + (dirToGroundedPosition * 2f);
        }

        internal override string GetName()
        {
            return "Leap Out Of Chasm";
        }
    }
}