using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Dash : Ability
    {
        public Dash(Marble subject) : base(subject)
        {
            _name = "Dash";
        }

        protected override EnemyAbilityUseRequirement[] GetEnemyUseRequirements()
        {
            EnemyAbilityUseRequirement[] requirements = new EnemyAbilityUseRequirement[1];

            requirements[0] = new BeNearPlayerRequirement(_subject);

            return requirements;
        }

        protected override void Activate()
        {
            Vector3 force = _subject.lookDirection * 10f;

            _subject.rb.AddForce(force, ForceMode.VelocityChange);
        }

        protected override bool IsAbleToActivate()
        {   
            // Dash is always able to activate.
            return true;
        }


    }


}

