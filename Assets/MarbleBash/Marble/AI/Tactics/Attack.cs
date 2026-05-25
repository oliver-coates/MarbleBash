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

            // _transtions.Add(leapFromChasm);
        }

        internal override void SetupTransitions()
        {
            
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
            return _brain.defaultTactic;
        }

        internal override string GetName()
        {
            return "Attack";
        }
    }


}

