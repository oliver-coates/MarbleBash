using UnityEngine;

namespace MarbleBash
{
    [System.Serializable]
    public class PlayerInstance : Marble
    {
        protected override void Setup()
        {
            _rigidbody = Player.rigidbody;
            _stats = new MarbleStats();
        }

        protected override Vector3 GetLookDirection()
        {
            return Player.look.pitchForward;
        }
    }
}