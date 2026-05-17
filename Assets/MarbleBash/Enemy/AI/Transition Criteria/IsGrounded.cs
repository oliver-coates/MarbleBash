namespace MarbleBash.Enemy
{

    internal class IsGrounded : TransitionCriteria
    {
        public IsGrounded(Marble marble) : base(marble) {}

        internal override bool Evaluate()
        {
            return _subject.movement.isGrounded;
        }
    }
}