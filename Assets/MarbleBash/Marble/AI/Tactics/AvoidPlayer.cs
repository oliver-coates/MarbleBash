using TMPro;
using UnityEngine;

namespace MarbleBash.Enemy
{

    internal class AvoidPlayer : Tactic
    {
        protected override void Start()
        {
            _duration = 5f;
        }

        internal override void SetupTransitions()
        {
            
        }


        protected override void Update()
        {
            _marble.movement.MoveInDirection(Vector3.zero);
        }

        protected override void OnTransition()
        {
            
        }

        protected override void OnDurationFinished()
        {
            
        }

        protected override Tactic GetNextTactic()
        {
            return _brain.defaultTactic;
        }

        internal override string GetName()
        {
            return "Avoid Player";
        }
    }


}

