using UnityEngine;

namespace MarbleBash.Abilities
{
    [System.Serializable]
    public class PlayerDash : Ability
    {
        public PlayerDash() : base()
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