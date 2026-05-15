using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash
{
    public class EnemyBrain : MarbleSubComponent
    {
        private EnemyMovement _movement;

        protected override void Initialise()
        {
            _movement = (EnemyMovement) _marble.movement;
        }

        private void Update()
        {
            _movement.SetPathingTarget(Player.transform.position);
        }


        
    }
}