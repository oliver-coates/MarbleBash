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
            Vector3 startForce = _subject.lookDirection * 250f;
            startForce.y = 0;

            _subject.rigidbody.AddForce(startForce);

            _subject.abilityEffects.AddEffect<Charging>();
        }

        protected override EnemyAbilityUseRequirement[] GetEnemyUseRequirements()
        {
            return new EnemyAbilityUseRequirement[0];
        }

        protected override bool IsAbleToActivate()
        {
            // TODO: Extract movement class out so isGrounded be accessed by '_subject.movement.distanceToGround'
            return Player.movement.isGrounded;
        }
    }


}

