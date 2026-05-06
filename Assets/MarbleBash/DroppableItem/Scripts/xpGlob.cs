using UnityEngine;

namespace MarbleBash
{

    public class xpGlob : MonoBehaviour
    {
        private float _timer;
        private Rigidbody _rb;
        private TrailRenderer _trailRenderer;

        private Vector3 _floatZone;

        public void Setup(Marble marble, float xp)
        {            
            _rb = this.GetComponentSafe<Rigidbody>();
            _trailRenderer = this.GetComponentSafe<TrailRenderer>();

            // Position:
            transform.position = marble.transform.position;
            
            SetupSize(xp);
            
            
        }

        private void SetupSize(float xp)
        {
            float size = Mathf.Clamp(xp / 100f, 0.01f, 1) * Random.Range(0.75f, 1.25f);

            _trailRenderer.startWidth = size;

            transform.localScale = new Vector3(size, size, size);
        }

    
        private void Update()
        {
            _timer += Time.deltaTime;

            // Vector3 directionToPlayer = (Player.transform.position - transform.position).normalized;
            // _rb.AddForce(speed * directionToPlayer, ForceMode.VelocityChange);

            Vector3 _targetPosition = Vector3.Lerp(_floatZone, Player.transform.position, _timer);

            _rb.MovePosition(Vector3.MoveTowards(transform.position, _targetPosition, 5f * Time.deltaTime * (1f + _timer)));

            // if (_timer < 0.5f)
            // {
            //     _rb.MovePosition(Vector3.MoveTowards(transform.position, _floatZone, Time.deltaTime * 5f));    
            // }
            // else
            // {
            //     float timerAdjuested = _timer - 0.5f;
            //     float speed = timerAdjuested * timerAdjuested * Time.deltaTime * 5f;

            //     _rb.MovePosition(Vector3.MoveTowards(transform.position, Player.transform.position, speed));

            //     if (Vector3.Distance(transform.position, Player.transform.position) < 0.5f)
            //     {
            //         Destroy(gameObject);
            //     }    
            // }
        }
    }


}

