using UnityEngine;

namespace MarbleBash.Enemy
{
    internal class ChargeAttack : Tactic
    {
        private const float MAX_DURATION = 2f;
        private const float CHARGE_DELAY = 0.15f;

        private bool _hasCharged;

        protected override void Start()
        {
            _duration = MAX_DURATION;
        
            _hasCharged = false;

            _marble.abilityTelegraph.Flash();
        }

        internal override void SetupTransitions()
        {
            // Attempt to jump at player when the dash ability is ready
            TacticTransition chargeAtPlayer = new TacticTransition(
                this,
                new TransitionCriteria[]
                {
                    new IsAbilityReady(_marble, "Charge"),
                    new IsWithinDistanceToPlayer(_marble, 4f, 8f),
                    _brain.AddCriteria<IsVelocityAlignedAgainstPlayer>()
                }
            );
            _brain.defaultTactic.AddTransition(chargeAtPlayer);
        }

        protected override void Update()
        {
            _brain.SetPathTarget(Player.movement.groundedPosition);

            if (_timeSinceStart > CHARGE_DELAY && !_hasCharged)
            {
                _hasCharged = true;
                _marble.abilities.AttemptActivateAbility("Charge");
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
            return "Charge Attack";
        }
    }
}

