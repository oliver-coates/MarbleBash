using UnityEngine;

namespace MarbleBash
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyInstance : Marble
    {
        protected override void Setup()
        {
            _rb = GetComponent<Rigidbody>();
        }

        protected override Vector3 GetLookDirection()
        {
            return (Player.transform.position - transform.position).normalized;
        }
    }


}
