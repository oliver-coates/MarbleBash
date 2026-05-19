using System;
using System.Collections.Generic;

namespace MarbleBash.Enemy
{
    
    internal class TacticTransition
    {
        protected EnemyBrain _brain;
        protected List<TransitionCriteria> _transitionCriteria;
        
        
        protected Tactic _tacticToTransition;

        internal TacticTransition(EnemyBrain brain, Type tacticType)
        {
            _brain = brain;
            _transitionCriteria = new List<TransitionCriteria>();
        
            // Ensure provided type is a tactic
            object o = Activator.CreateInstance(tacticType);
            if (o is Tactic tactic)
            {
                _tacticToTransition = tactic;
            }
        }

        internal TacticTransition(EnemyBrain brain, Type tacticType, TransitionCriteria[] criteria)
        {
            _brain = brain;
            _transitionCriteria = new List<TransitionCriteria>(criteria);
        
            // Ensure provided type is a tactic
            object o = Activator.CreateInstance(tacticType);
            if (o is Tactic tactic)
            {
                _tacticToTransition = tactic;
            }
        }

        internal void Check()
        {
            foreach (TransitionCriteria criteria in _transitionCriteria)
            {
                if (criteria.Evaluate())
                {
                    _brain.TransitionToTactic(_tacticToTransition);
                    return;
                }
            }
        }

        internal void AddNewCriteria(TransitionCriteria criteria)
        {
            _transitionCriteria.Add(criteria);
        }

    }
}