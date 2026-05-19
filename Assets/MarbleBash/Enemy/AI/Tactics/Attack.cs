namespace MarbleBash.Enemy
{

    internal class Attack : Tactic
    {
        protected override void Start()
        {
            _duration = 5f;

            TacticTransition overChasm = new TacticTransition(
                _brain, 
                typeof(LeapOutOfChasm),
                new TransitionCriteria[] {new IsOverChasm(_brain)}
            );

            _transtions.Add(overChasm);
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

