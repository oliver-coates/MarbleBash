namespace MarbleBash.Enemy
{
    
    internal abstract class TransitionCriteria
    {
        protected Marble _subject;
        
        internal void Initialise(Marble marble)
        {
            _subject = marble;
        }

        internal abstract bool Evaluate();
    }
}