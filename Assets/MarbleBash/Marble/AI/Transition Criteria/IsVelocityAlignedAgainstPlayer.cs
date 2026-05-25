using UnityEngine;

namespace MarbleBash.Enemy
{

    internal class IsVelocityAlignedAgainstPlayer : TransitionCriteria
    {
        internal override bool Evaluate()
        {
            return Vector3.Dot(_subject.rigidbody.linearVelocity.normalized, _subject.movement.lookDirection) > 0.75f;
        }
    }
}