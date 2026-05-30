namespace MarbleBash.Enemy
{

    internal class ThrowRollerBomb : Tactic
    {
        protected override void Start()
        {
            _duration = 1f;

            _marble.abilities.AttemptActivateAbility("Throw Roller Bomb");
        }

        internal override void SetupTransitions()
        {
            // Transition into this when this ability is ready
            TacticTransition readyToThrowBombTransition = new TacticTransition(
                this,
                new TransitionCriteria[]
                {
                    new IsAbilityReady(_marble, "Throw Roller Bomb")
                }
            );

            _brain.defaultTactic.AddTransition(readyToThrowBombTransition);
        }


        protected override void Update()
        {
            // _brain.SetPathTarget(Player.movement.groundedPosition);
        }

        protected override void OnTransition()
        {
            
        }

        protected override void OnDurationFinished()
        {
            
        }

        protected override Tactic GetNextTactic()
        {
            return _brain.defaultTactic;
        }

        internal override string GetName()
        {
            return "Throw Roller Bomb";
        }
    }


}

