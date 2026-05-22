using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Charge : Ability
    {
        public Charge(Marble subject) : base(subject)
        {
            _name = "Charge";
        }

        protected override void Activate()
        {
            Vector3 startForce = _subject.movement.lookDirection * 250f;
            startForce.y = 0;

            _subject.rigidbody.AddForce(startForce);

            _subject.abilityEffects.AddEffect<Charging>();
        }

        protected override bool IsAbleToActivate()
        {
            float alignedVelocity = Vector3.Dot(_subject.movement.lookDirection, _subject.movement.cachedVelocity.normalized);

            return _subject.movement.isGrounded && (alignedVelocity > 0.90f);
        }
    }


}

