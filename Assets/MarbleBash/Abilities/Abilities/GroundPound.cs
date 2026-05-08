using UnityEngine;

namespace MarbleBash.Abilities
{
    public class GroundPound : Ability
    {

        private const float HEIGHT_ABOVE_GROUND_TO_ACTIVATE = 4;

        public GroundPound(Marble subject) : base(subject)
        {
            _name = "Ground Pound";
        }

        protected override void Activate()
        {
            Vector3 force = Vector3.down * 350f;
            _subject.rigidbody.AddForce(force);

            _subject.abilityEffects.AddEffect<Pounding>();
        }

        protected override EnemyAbilityUseRequirement[] GetEnemyUseRequirements()
        {
            return new EnemyAbilityUseRequirement[0];
        }

        protected override bool IsAbleToActivate()
        {
            // TODO: Extract movement class out so distanceToGround can be accessed by '_subject.movement.distanceToGround'
            return Player.movement.distanceToGround > HEIGHT_ABOVE_GROUND_TO_ACTIVATE;
        }
    }
}

