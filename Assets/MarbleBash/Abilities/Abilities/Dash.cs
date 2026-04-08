using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Dash : Ability
    {
        public Dash(Marble subject) : base(subject)
        {
            _name = "Dash";
        }

        protected override void Activate()
        {
            Vector3 force = _subject.lookDirection * 10f;

            MarbleBash.Player.rigidbody.AddForce(force, ForceMode.VelocityChange);
        }

        protected override bool IsAbleToActivate()
        {   
            // Dash is always able to activate.
            return true;
        }


    }


}

