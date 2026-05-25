using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash.Enemy
{
    public class EnemyBrain : MarbleSubComponent
    {
        #region References
        private EnemyMovement _movement;
        internal EnemyMovement movement
        {
            get
            {
                return _movement;
            }
        }
        internal Marble marble
        {
            get
            {
                return _marble;
            }
        }
        #endregion

        #region Tactics
        private Tactic _currentTactic;
        private Tactic _defaultTactic;
        internal Tactic defaultTactic => _defaultTactic;
        private Tactic[] _tactics;
        private Dictionary<string, Tactic> _nameToTacticDict;
        private Dictionary<Type, TransitionCriteria> _transitionCriteriaDict;
        private List<TacticTransition> _generalTransitions;
        #endregion


        // Note this Initialise method, as part of the MarbleSubComoponent, is called but not used.
        // Instead the other Initialise method is called by the EnemyBrainInitialiser to prime this class with its tactics
        protected override void Initialise()
        {
        }

        internal void Initialise(Tactic[] tactics)
        {
            _movement = (EnemyMovement) _marble.movement;
            
            _tactics = tactics;
            _transitionCriteriaDict = new ();
            _generalTransitions = new ();
            
            _defaultTactic = tactics[0];
            _currentTactic = tactics[0];

            // Setup dictionary:
            _nameToTacticDict = new ();
            foreach (Tactic tactic in _tactics)
            {
                _nameToTacticDict.Add(tactic.GetName(), tactic);
            }

            // Get each newly created tactic to set up their transitions:
            foreach (Tactic tactic in _tactics)
            {
                tactic.SetupTransitions();
            }

            this.enabled = true;
            StartTactic(_defaultTactic);
        }

        private void Update()
        {
            _currentTactic.Tick();

            foreach (TacticTransition tacticTransition in _generalTransitions)
            {
                if (tacticTransition.Check())
                {
                    TransitionToTactic(tacticTransition.toTransitionTo);
                    break;
                }
            }
        }

        internal void StartTactic(Tactic tactic)
        {
            _currentTactic = tactic;
            _currentTactic.Begin();
        }

        internal void FlowOnToNextTactic(Tactic newTactic)
        {
            _currentTactic.Finish(Tactic.FinishReason.DurationExpired);

            _currentTactic = newTactic;
            _currentTactic.Begin();
        }

        internal void TransitionToTactic(Tactic newTactic)
        {
            _currentTactic.Finish(Tactic.FinishReason.TransitionOccured);

            newTactic.Begin();
            _currentTactic = newTactic;
        }

        internal void SetPathTarget(Vector3 position)
        {
            _movement.SetPathingTarget(position);            
        } 

        /// <summary>
        /// Creates a new transition criteria of the provided type.
        /// Returns the transition criteria object.
        /// If a transition object is not already created, a new one will be made.
        /// </summary>
        internal TransitionCriteria AddCriteria<T>() where T : TransitionCriteria
        {
            Type transitionType = typeof(T);

            if (_transitionCriteriaDict.ContainsKey(transitionType))
            {
                return _transitionCriteriaDict[transitionType];
            }

            TransitionCriteria newCriteria = (TransitionCriteria) Activator.CreateInstance(transitionType);
            newCriteria.Initialise(_marble);
            _transitionCriteriaDict.Add(transitionType, newCriteria);

            return newCriteria;
        }

        internal bool Evaluate<T>() where T : TransitionCriteria
        {
            return _transitionCriteriaDict[typeof(T)].Evaluate();
        }

        /// <summary>
        /// Adds a tactic transition that will be evaluated always regardless of 
        /// which tactic is currently selected.
        /// </summary>
        internal void AddGeneralTransition(TacticTransition transition)
        {
            _generalTransitions.Add(transition);
        } 
    }
}