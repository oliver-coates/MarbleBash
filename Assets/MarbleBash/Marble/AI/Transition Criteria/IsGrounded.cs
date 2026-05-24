namespace MarbleBash.Enemy
{

    internal class IsGrounded : TransitionCriteria
    {
        public IsGrounded(EnemyBrain brain) : base (brain) {}

        internal override bool Evaluate()
        {
            return _subject.movement.isGrounded;
        }
    }
}