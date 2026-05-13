using KahuInteractive.HassleFreeConfig;
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
            Vector3 force = _subject.movement.lookDirection * Configuration.Read("dash_force_base");

            _subject.rigidbody.AddForce(force);
        }

        protected override bool IsAbleToActivate()
        {   
            // Dash is always able to activate.
            return true;
        }


    }


}

