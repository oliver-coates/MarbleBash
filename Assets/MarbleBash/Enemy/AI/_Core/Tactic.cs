using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash.Enemy
{
    internal abstract class Tactic
    {
        protected float _duration;
        protected EnemyBrain _brain;

        protected List<TacticTransition> _transtions;

        internal void Initialise(EnemyBrain brain)
        {
            _brain = brain;
            _duration = 1f;
            _transtions = new();
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

            foreach (TacticTransition tacticTransition in _transtions)
            {
                tacticTransition.Check();
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
