using UnityEngine;

namespace MarbleBash.Enemy
{

    public class Attack : Tactic
    {
        protected override void Start()
        {
            _duration = 5f;
        }

        protected override void Finished()
        {
        }

        protected override void Update()
        {
            _brain.SetPathTarget(Player.movement.groundedPosition);
        }
    }


}

