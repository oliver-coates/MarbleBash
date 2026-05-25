using UnityEngine;

namespace MarbleBash.Enemy
{

    internal class IsWithinDistanceToPlayer : TransitionCriteria
    {
        private float _minDistance;
        private float _maxDistance;
        internal IsWithinDistanceToPlayer(Marble marble, float min, float max)
        {
            Initialise(marble);
            _minDistance = min;
            _maxDistance = max;
        }

        internal override bool Evaluate()
        {
            float distance = Vector3.Distance(_subject.transform.position, Player.transform.position);

            return (distance > _minDistance) && (distance < _maxDistance);
        }
    }
}