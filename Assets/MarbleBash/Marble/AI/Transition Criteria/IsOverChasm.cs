using UnityEngine;

namespace MarbleBash.Enemy
{

    internal class IsOverChasm : TransitionCriteria
    {
        internal override bool Evaluate()
        {
            return _subject.movement.distanceToGround == MarbleMovement.GROUNDED_RAYCAST_DOWN_MAXIMUM_DISTANCE;
        }
    }
}