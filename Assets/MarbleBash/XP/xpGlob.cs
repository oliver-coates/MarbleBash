using UnityEngine;

namespace MarbleBash
{

    public class xpGlob : MonoBehaviour
    {
        private float _timer;
        private Rigidbody _rb;
        private TrailRenderer _trailRenderer;

        public void Setup(Marble marble, float xp)
        {            
            _rb = this.GetComponentSafe<Rigidbody>();
            _trailRenderer = this.GetComponentSafe<TrailRenderer>();

            // Position:
            transform.position = marble.transform.position;
            
            SetupSize(xp);
            
            // Throw
            Vector3 dir = GetThrowDirection();
            float throwForce = 10 * Random.Range(0.75f, 1f);

            _rb.AddForce(dir * throwForce, ForceMode.VelocityChange);
        }

        private void SetupSize(float xp)
        {
            float size = Mathf.Clamp(xp / 100f, 0.01f, 1) * Random.Range(0.75f, 1.25f);

            _trailRenderer.startWidth = size;

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

            // Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;
            // _rb.AddForce(speed * directionToPlayer, ForceMode.VelocityChange);

            if (_timer > 0.5f)
            {
                float timerAdjuested = _timer - 0.5f;
                float speed = timerAdjuested * timerAdjuested * Time.deltaTime * 5f;

                _rb.Move(Vector3.MoveTowards(transform.position, Player.transform.position, speed), transform.rotation);

                if (Vector3.Distance(transform.position, Player.transform.position) < 0.5f)
                {
                    Destroy(gameObject);
                }    
            }
        }
    }


}

