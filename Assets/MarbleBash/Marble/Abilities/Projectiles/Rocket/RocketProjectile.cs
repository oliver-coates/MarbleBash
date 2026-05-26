using KahuInteractive.VisualFX;
using UnityEngine;

namespace MarbleBash
{

    public class RocketProjectile : MonoBehaviour
    {
        public void Initialise(Vector3 startPos, Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - startPos).normalized;
        
            transform.position = startPos;
            transform.rotation = Quaternion.LookRotation(direction);
        }

        private void Update()
        {
            float speed = Time.deltaTime * 25f;
            transform.position = transform.position + transform.forward * speed;
        }

        private void OnCollisionEnter(Collision c)
        {
            OneShotEffectData data = new OneShotEffectData("Ground Pound", c.contacts[0].point, Quaternion.identity, 3f);
            VFX.Play(data);

            Destroy(gameObject);
        }
    }


}

