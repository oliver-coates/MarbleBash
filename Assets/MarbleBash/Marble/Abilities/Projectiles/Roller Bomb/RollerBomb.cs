using System;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash.Abilities
{
    public class RollerBomb : MonoBehaviour
    {

        private const float TIME_TO_ACTIVATE = 0.5f;


        private Rigidbody _rb;

        private float _timeAlive;
        private bool _hasActivated;

        public void Initialise()
        {
            _rb = this.GetComponentSafe<Rigidbody>();

            _timeAlive = 0f;
            _hasActivated = false;
        
            gameObject.layer = LayerMask.NameToLayer("Debris");

            Throw();
        }

        private void Throw()
        {
            Vector2 random2d = UnityEngine.Random.onUnitCircle;
            float randomUp = UnityEngine.Random.Range(1.5f, 3f);

            Vector3 throwDir = new Vector3(random2d.x, randomUp, random2d.y).normalized;
            float throwForce = 50 * UnityEngine.Random.Range(0.75f, 1.25f);

            _rb.AddForce(throwDir * throwForce);
        }

        private void Update()
        {
            _timeAlive += Time.deltaTime;

            if (_hasActivated)
            {
                ChasePalyer();
            }
            else
            {
                if (_timeAlive > TIME_TO_ACTIVATE)
                {
                    Activate();
                }
            }
            
        }

        private void ChasePalyer()
        {
            Vector3 dirToPlayer = (Player.transform.position - transform.position).normalized;

            _rb.AddForce(dirToPlayer * 100f * Time.deltaTime);
        }

        private void Activate()
        {
            _hasActivated = true;
            gameObject.layer = LayerMask.NameToLayer("Default");           
        }
    }
}
