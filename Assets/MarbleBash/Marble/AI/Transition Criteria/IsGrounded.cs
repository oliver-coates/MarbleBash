namespace MarbleBash.Enemy
{

    internal class IsGrounded : TransitionCriteria
    {
        internal override bool Evaluate()
        {
            return _subject.movement.isGrounded;
        }
    }
}