using UnityEngine;

namespace MarbleBash.Enemy
{

    internal class IsNotMovingAwayFromPlayer : TransitionCriteria
    {
        internal IsNotMovingAwayFromPlayer(EnemyBrain brain) : base(brain)
        {
        }


        internal override bool Evaluate()
        {
            return Vector3.Dot(_subject.rigidbody.linearVelocity.normalized, _subject.movement.lookDirection) > 0.1f;
        }
    }
}