using UnityEngine;

namespace MarbleBash
{

    public class xpGlob : MonoBehaviour
    {
        private Rigidbody _rb;
        private float _timer;

        public void Setup(Marble marble, float xp)
        {
            _rb = this.GetComponentSafe<Rigidbody>();
            
            // Position:
            transform.position = marble.transform.position;
            
            SetupSize(xp);
            
            // Throw
            Vector3 dir = GetThrowDirection();
            float throwForce = Random.Range(2f, 4f);
            _rb.AddForce(dir * throwForce, ForceMode.VelocityChange);
        }

        private void SetupSize(float xp)
        {
            float size = xp / 25;
            transform.localScale = new Vector3(size, size, size);
        }

        private Vector3 GetThrowDirection()
        {
            Vector2 randomOnCircle = Random.onUnitCircle * Random.Range(0.75f, 1f);
            Vector3 direction = new Vector3(randomOnCircle.x, 1, randomOnCircle.y);
        
            return direction.normalized;
        }
    
        private void Update()
        {
            _timer += Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, _timer * Time.deltaTime * 2.5f);

            if (_timer > 1f && Vector3.Distance(transform.position, Player.transform.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }
    }


}

