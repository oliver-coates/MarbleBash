namespace MarbleBash.Enemy
{
    
    internal abstract class TransitionCriteria
    {
        protected Marble _subject;
        
        internal TransitionCriteria(EnemyBrain brain)
        {
            _subject = brain.marble;
        }

        internal abstract bool Evaluate();
    }
}