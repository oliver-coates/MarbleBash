using UnityEngine;

namespace MarbleBash
{
    [System.Serializable]
    public class PlayerInstance : Marble
    {
        /// <summary>
        /// Setup for player marble is easy, just pull data from the player singleton.
        /// </summary>
        protected override void Setup()
        {
            _rb = Player.rigidbody;
            _stats = new MarbleStats();
        }

        protected override Vector3 GetLookDirection()
        {
            return Player.look.pitchForward;
        }

    }
}