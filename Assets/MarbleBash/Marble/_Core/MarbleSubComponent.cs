using UnityEngine;

namespace MarbleBash
{
    
    public abstract class MarbleSubComponent : MonoBehaviour
    {
        protected Marble _marble;
        internal void Initialise(Marble marble)
        {
            _marble = marble;
            Initialise();
        }

        protected abstract void Initialise();
    }
}