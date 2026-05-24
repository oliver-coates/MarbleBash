namespace MarbleBash.Enemy
{

    internal class Attack : Tactic
    {
        protected override void Start()
        {
            _duration = 5f;

            // TacticTransition leapFromChasm = new TacticTransition(
            //     _brain, 
            //     typeof(LeapOutOfChasm),
            //     new TransitionCriteria[] 
            //     {
            //         new IsOverChasm(_brain)
            //     }
            // );

            // TacticTransition dashAtPlayer = new TacticTransition(
            //     _brain,
            //     typeof(DashAttack),
            //     new TransitionCriteria[]
            //     {
            //         new IsAbilityReady(_brain, "Dash"),
            //         new IsWithinDistanceToPlayer(_brain, 0.75f, 3f),
            //         new IsNotMovingAwayFromPlayer(_brain),
            //     }
            // );
            
            // _transtions.Add(leapFromChasm);
            // _transtions.Add(dashAtPlayer);
        }


        protected override void Update()
        {
            _brain.SetPathTarget(Player.movement.groundedPosition);
        }

        protected override void OnTransition()
        {
            
        }

        protected override void OnDurationFinished()
        {
            
        }

        protected override Tactic GetNextTactic()
        {
            return new Attack();
        }
    }


}

