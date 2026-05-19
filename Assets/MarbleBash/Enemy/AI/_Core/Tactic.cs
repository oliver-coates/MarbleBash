using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash.Enemy
{
    internal abstract class Tactic
    {
        internal enum FinishReason
        {
            DurationExpired = 0,
            TransitionOccured=1,
        }
        protected float _duration;
        protected EnemyBrain _brain;
        protected EnemyInstance _marble;

        protected List<TacticTransition> _transtions;

        internal void Initialise(EnemyBrain brain)
        {
            _brain = brain;
            _marble = (EnemyInstance) brain.marble;

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
                _brain.FlowOnToNextTactic(GetNextTactic());
                return;
            }

            foreach (TacticTransition tacticTransition in _transtions)
            {
                tacticTransition.Check();
            }
        }

        internal void Finish(FinishReason reason)
        {
            switch (reason)
            {
                case FinishReason.DurationExpired:
                    OnDurationFinished();
                    
                    return;
                case FinishReason.TransitionOccured:
                    OnTransition();
                    return;
            }
        }

        protected abstract void Start();

        protected abstract void Update();

        /// <summary>
        /// Called when this tactic is transitioned away from.
        /// Not called when the duration of this target ends.
        /// </summary>
        protected abstract void OnTransition();

        /// <summary>
        /// Called when the duration of this tactic ends.
        /// </summary>
        protected abstract void OnDurationFinished();

        /// <summary>
        /// Gets the next tactic that this tactic will 'flow on to' when it's duration expires.
        /// </summary>
        /// <returns></returns>
        protected abstract Tactic GetNextTactic();
    }
}
