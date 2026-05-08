using System;
using UnityEngine;

namespace MarbleBash.Abilities
{
    [System.Serializable]
    public abstract class AbilityEffect
    {
        private Marble _subject;
        public Marble subject
        {
            get
            {
                return _subject;
            }
        }

        [SerializeField] protected float _duration;
        public float duration
        {
            get
            {
                return _duration;
            }	
        }
        
        [SerializeField] protected float _timeElapsed;
        public float timeElapsed
        {
            get
            {
                return _timeElapsed;
            }	
        }

        [SerializeField] protected float _strength;
        public float strength
        {
            get
            {
                return _strength;
            }	
        }

        [SerializeField] private bool _isFinished;
        public bool isFinished
        {
            get
            {
                return _isFinished;
            }	
        }

        public AbilityEffect()
        {
            _isFinished = false;
            _timeElapsed = 0f;
        }

        public void Initialise(Marble subject)
        {
            _subject = subject;
            
            Start();
        }

        /// <summary>
        /// Called every tick that this effect is active.
        /// </summary>
        internal void Tick()
        {
            _timeElapsed += Time.deltaTime;

            if (_timeElapsed >= _duration)
            {
                StopEffect();
            }

            Update();
        }

        /// <summary>
        /// Called when this effect is applied.
        /// </summary>
        protected abstract void Start();

        /// <summary>
        /// Applies the effects that this effect does (For example, reduces health if a poison effect)
        /// Called every tick that this effect is active.
        /// </summary>
        protected abstract void Update();

        /// <summary>
        /// Ends the effect. 
        /// Will be called once duration runs out. Call this to remove the effect early if desired.
        /// 
        /// </summary>
        protected void StopEffect()
        {
            _isFinished = true;
            Finished();
        }

        /// <summary>
        /// Called once this effect finished.
        /// </summary>
        protected abstract void Finished();        
    }
    
}
