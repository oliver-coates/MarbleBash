using System;
using UnityEngine;

namespace MarbleBash.Enemy
{
    public abstract class Tactic
    {
        protected float _duration;
        protected EnemyBrain _brain;

        internal void Initialise(EnemyBrain brain)
        {
            _brain = brain;
            _duration = 1f;
            Start();
        }        

        internal void Tick()
        {
            _duration -= Time.deltaTime;
            Update();

            if (_duration < 0)
            {
                Finish();
            }
        }

        internal void Finish()
        {
            Finished();
        }

        protected abstract void Start();

        protected abstract void Update();

        protected abstract void Finished();

        
    }
}
