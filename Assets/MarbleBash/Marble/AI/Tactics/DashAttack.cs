using UnityEngine;

namespace MarbleBash.Enemy
{
    internal class DashAttack : Tactic
    {
        private const float MAX_DURATION = 0.8f;
        private const float FLASH_TIME = 0.2f;
        private const float JUMP_TIME = 0.2f;

        // Time when we should dash
        private float _dashTime;
        // Time when we should flash
        private bool _doFlash;
        private float _flashTime;
        // Whether we should jump and at what time
        private bool _doJump;
        private float _jumpTime;

        protected override void Start()
        {
            _duration = MAX_DURATION;
        
            _dashTime = Random.Range(FLASH_TIME, MAX_DURATION);

            _doFlash = true;
            _flashTime = _dashTime - FLASH_TIME;

            _doJump = Random.Range(0, 1f) < 0.33f;
            if (_doJump)
            {
                _jumpTime = _dashTime - JUMP_TIME;
            }
        }

        internal override void SetupTransitions()
        {
            // Attempt to jump at player when the dash ability is ready
            TacticTransition dashAtPlayer = new TacticTransition(
                this,
                new TransitionCriteria[]
                {
                    new IsAbilityReady(_marble, "Dash"),
                    new IsWithinDistanceToPlayer(_marble, 0.75f, 3f),
                    _brain.AddCriteria<IsNotMovingAwayFromPlayer>()
                }
            );
            _brain.defaultTactic.AddTransition(dashAtPlayer);
        }

        protected override void Update()
        {
            _brain.SetPathTarget(Player.movement.groundedPosition);

            if (_timeSinceStart > _dashTime)
            {
                _marble.rigidbody.linearVelocity *= 0.75f;
                _marble.abilities.AttemptActivateAbility("Dash");
                EndTactic();
            }
            if (_doFlash && _timeSinceStart > _flashTime)
            {
                // Flash
                _marble.abilityTelegraph.Flash();
                _doFlash = false;
            }
            if (_doJump && _timeSinceStart > _jumpTime)
            {
                _brain.movement.AttemptJump();
                _doJump = false;
            }
        }

        protected override void OnDurationFinished()
        {
        }

        protected override void OnTransition()
        {
        }


        protected override Tactic GetNextTactic()
        {
            return _brain.defaultTactic;
        }

        internal override string GetName()
        {
            return "Dash Attack";
        }
    }
}

