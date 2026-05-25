using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash.Abilities
{
    public class GroundPound : Ability
    {

        private const float HEIGHT_ABOVE_GROUND_TO_ACTIVATE = 4;

        private float _downForce;

        public GroundPound(Marble subject) : base(subject)
        {
            _name = "Ground Pound";
            _downForce = Configuration.Read("ground_pound_initial_force");
        }

        protected override void Activate()
        {
            Vector3 force = Vector3.down * _downForce;
            _subject.rigidbody.AddForce(force);

            _subject.statusEffects.AddEffect<Pounding>();
        }
        
        protected override bool IsAbleToActivate()
        {
            return _subject.movement.distanceToGround > HEIGHT_ABOVE_GROUND_TO_ACTIVATE;
        }
    }
}

