using UnityEngine;

namespace MarbleBash.Enemy
{

    internal class Attack : Tactic
    {
        protected override void Start()
        {
            _duration = 5f;

            TacticTransition overChasm = new TacticTransition(_brain, typeof(LeapOutOfChasm));
            overChasm.AddNewCriteria(new IsOverChasm(_brain.marble));
            _transtions.Add(overChasm);
        }

        protected override void Finished()
        {
            _brain.ChangeTactic<Attack>();
        }

        protected override void Update()
        {
            _brain.SetPathTarget(Player.movement.groundedPosition);
        }
    }


}

