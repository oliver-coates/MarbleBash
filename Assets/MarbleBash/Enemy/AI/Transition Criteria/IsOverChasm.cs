using UnityEngine;

namespace MarbleBash.Enemy
{

    internal class IsOverChasm : TransitionCriteria
    {
        public IsOverChasm(Marble marble) : base(marble) {}

        internal override bool Evaluate()
        {
            return _subject.movement.distanceToGround == MarbleMovement.GROUNDED_RAYCAST_DOWN_MAXIMUM_DISTANCE;
        }
    }
}