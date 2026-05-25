using System;
using System.Collections.Generic;

namespace MarbleBash.Enemy
{
    
    internal class TacticTransition
    {
        protected List<TransitionCriteria> _transitionCriteria;
        protected Tactic _tacticToTransition;
        internal Tactic toTransitionTo => _tacticToTransition;

        internal bool enabled;

        internal TacticTransition(Tactic toTransitionTo, TransitionCriteria[] criteria)
        {
            enabled = true;
            _transitionCriteria = new List<TransitionCriteria>(criteria);
            _tacticToTransition = toTransitionTo;
        }

        internal bool Check()
        {
            if (!enabled)
            {
                return false;
            }

            foreach (TransitionCriteria criteria in _transitionCriteria)
            {
                if (criteria.Evaluate() == false)
                {
                    return false;
                }
            }

            return true;
        }

        internal void AddNewCriteria(TransitionCriteria criteria)
        {
            _transitionCriteria.Add(criteria);
        }

    }
}