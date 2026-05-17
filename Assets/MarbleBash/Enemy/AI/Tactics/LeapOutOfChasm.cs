using MarbleBash.Enemy;
using UnityEditorInternal;
using UnityEngine;

namespace MarbleBash
{

    internal class LeapOutOfChasm : Tactic
    {
        protected override void Start()
        {
            _duration = 2f;

            TacticTransition backOnGround = new TacticTransition(_brain, typeof(Attack));
            backOnGround.AddNewCriteria(new IsGrounded(_brain.marble));
            _transtions.Add(backOnGround);
            Debug.Log($"leaping!");
        }

        protected override void Finished()
        {
        }

        protected override void Update()
        {
        }
    }
}