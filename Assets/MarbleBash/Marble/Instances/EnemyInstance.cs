using UnityEngine;

namespace MarbleBash
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyInstance : Marble
    {
        protected override void Setup()
        {
            _rb = GetComponent<Rigidbody>();

            // In future, enemies should be setup with a method:
            _stats = new MarbleStats();
        }

        protected override Vector3 GetLookDirection()
        {
            return (Player.transform.position - transform.position).normalized;
        }
    }


}
