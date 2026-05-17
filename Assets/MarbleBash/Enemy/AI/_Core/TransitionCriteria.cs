namespace MarbleBash.Enemy
{
    
    internal abstract class TransitionCriteria
    {
        protected Marble _subject;
        
        internal TransitionCriteria(Marble marble)
        {
            _subject = marble;
        }

        internal abstract bool Evaluate();
    }
}