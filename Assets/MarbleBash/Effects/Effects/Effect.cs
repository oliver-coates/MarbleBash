using System;
using UnityEngine;

namespace MarbleBash.Effects
{
    public abstract class Effect
    {
        protected Marble target;

        [SerializeField] private float _duration;
        public float duration
        {
            get
            {
                return _duration;
            }	
        }
        
        [SerializeField] private float _timeElapsed;
        public float timeElapsed
        {
            get
            {
                return _timeElapsed;
            }	
        }

        [SerializeField] private float _strength;
        public float strength
        {
            get
            {
                return _strength;
            }	
        }

        public Action<Effect> onEffectFinished;


        public Effect(Marble target, float length, float strength)
        {
            this.target = target;
            _strength = strength;
            _duration = length; 
            _timeElapsed = 0f;

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
                Finished();
                onEffectFinished?.Invoke(this);
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
        /// Called once this effect finished.
        /// </summary>
        protected abstract void Finished();        
    }
    
}
