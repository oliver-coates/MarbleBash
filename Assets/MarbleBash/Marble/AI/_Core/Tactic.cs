using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarbleBash.Enemy
{
    internal abstract partial class Tactic
    {
        internal enum FinishReason
        {
            DurationExpired = 0,
            TransitionOccured = 1,
        }

        protected float _duration;
        protected float _timeSinceStart;

        protected EnemyBrain _brain;
        protected EnemyInstance _marble;
        protected List<TacticTransition> _transtions;

        /// <summary>
        /// Sets up the tactic.
        /// Called when the tactic is first created.
        /// </summary>
        internal void Initialise(EnemyBrain brain)
        {
            _brain = brain;
            _marble = (EnemyInstance) brain.marble;

            _transtions = new();
        }

        /// <summary>
        /// Starts this tactic
        /// Resets this tactic's internal clock and calls the start method.
        /// Called whenever we enter into this tactic state.
        /// </summary>
        internal void Begin()
        {
            _duration = 1f; // This duration is typically overwritten in the start method, which is a bit sloppy, we could consider refactoring this.
            _timeSinceStart = 0f;            
        
            Start();
        }

        internal void Tick()
        {
            _timeSinceStart += Time.deltaTime;
            Update();

            if (_timeSinceStart >= _duration)
            {
                EndTactic();
                return;
            }

            foreach (TacticTransition tacticTransition in _transtions)
            {
                if (tacticTransition.Check())
                {
                    _brain.TransitionToTactic(tacticTransition.toTransitionTo);
                    return;    
                }
                
            }
        }

        internal void EndTactic()
        {
            _brain.FlowOnToNextTactic(GetNextTactic());
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

        internal abstract void SetupTransitions();

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
    
        internal abstract string GetName();

        internal void AddTransition(TacticTransition transition)
        {
            _transtions.Add(transition);
        }
    
        internal static Tactic CreateFromName(string name)
        {
            switch (name)
            {
                case "Attack":
                    return new Attack();
                
                case "Dash Attack":
                    return new DashAttack();
                
                case "Leap Out Of Chasm":
                    return new LeapOutOfChasm();
                
                case "Pound Attack":
                    return new PoundAttack();
                
                case "Charge Attack":
                    return new ChargeAttack();
                
                case "Avoid Player":
                    return new AvoidPlayer();
                
                case "Throw Roller Bomb":
                    return new ThrowRollerBomb();

                default:
                    Debug.LogError($"Unhandled tactic name '{name}'");
                    return null;
            }
        }
    }
}
