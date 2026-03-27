using UnityEngine;

namespace MarbleBash.Abilities
{

    public class Dash : Ability
    {
        public Dash() : base()
        {
            _name = "Dash";
        }


        protected override void Activate()
        {
            Vector3 force = Player.look.pitchForward * 10f;

            Player.rigidbody.AddForce(force, ForceMode.VelocityChange);
        }

        protected override bool IsAbleToActivate()
        {   
            // Dash is always able to activate.
            return true;
        }
    }
}