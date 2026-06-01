using KahuInteractive.HassleFreeConfig;
using KahuInteractive.VisualFX;
using TMPro;
using UnityEngine;

namespace MarbleBash.Abilities
{
    [System.Serializable]
    public class Pounding : AbilityEffect
    {
        private const float MINIMUM_FORCE = 5f;
        private MasksConfig _masks;
        private LayerMask _toDamageMask;

        protected override void Start()
        {
            _masks = Configuration.Get<MasksConfig>();
            if (subject.isPlayer)
            {
                _toDamageMask = _masks.enemyMarbles;
            }
            else
            {
                _toDamageMask = _masks.playerMarble;
            }
            
            _duration = 10f;

            subject.collisionHandler.OnCollisionGround += HitGround;
            if (subject.isPlayer)
            {
                subject.collisionHandler.AssignDamageListener(HitMarble);    
            }
            else
            {
                subject.collisionHandler.OnCollisionMarble += HitMarble;
            }

            subject.collisionHandler.OnCollisionAll += Test;
        }

        private void Test(Collision collision)
        {
            Debug.Log($"COllided with: {collision.gameObject.name}");
        }

        private void PoundMarble(Marble toPound, float force)
        {
            if (toPound.stats.isImmuneFromDamage)
            {
                return;                
            }   

            // Randomise knockback direction - It doesn't really matter here as the 
            // physics engine is going to freak out from the player pounding onto the enemy.
            Vector3 knockbackDir = new Vector3(Random.Range(0,1f), 0, Random.Range(0, 1f)).normalized;

            DamageManager.ApplyDamage(subject, toPound, force * 1.5f, knockbackDir);

            Vector3 velocity = subject.rigidbody.linearVelocity;
            velocity.x = velocity.x * 0.66f;
            velocity.z = velocity.z * 0.66f;
            velocity.y = -subject.movement.cachedVelocity.y * 0.5f;

            subject.transform.position += Vector3.up * 0.5f;

            subject.rigidbody.linearVelocity = velocity;
        }

        private void PoundGround(Vector3 pos)
        {
            float impactVelocity = subject.movement.cachedSpeed;
            float size = GetDamageRadius();

            Collider[] hits = Physics.OverlapSphere(pos, size, _toDamageMask);
            foreach (Collider hit in hits)
            {
                Marble hitMarble = hit.gameObject.GetComponent<Marble>();
                float distanceL = 1f - (Vector3.Distance(pos, hitMarble.transform.position) / size);

                float damage = impactVelocity * distanceL;

                Vector3 knockbackDir = hitMarble.transform.position - subject.transform.position;
                knockbackDir.y = 3f;

                DamageManager.ApplyDamage(subject, hitMarble, damage, knockbackDir, 1.25f);
            }
            
            VFX.Play(new OneShotEffectData("Ground Pound", pos, Quaternion.identity, size));
        }

        protected override void Update()
        {
            Vector3 force = Vector3.down * 1000 * Time.deltaTime;
            subject.rigidbody.AddForce(force);

            // if (subject.rigidbody.linearVelocity.y > -0.25f)
            // {
            //     HitGround();
            // }
        }

        protected override void Finished()
        {
            Debug.Log($"Finished pounding effect");
            subject.collisionHandler.OnCollisionGround -= HitGround;
            if (subject.isPlayer)
            {
                subject.collisionHandler.UnassignDamageListener();            
            }
            else
            {
                subject.collisionHandler.OnCollisionMarble -= HitMarble;
            }

            subject.collisionHandler.OnCollisionAll -= Test;

        }

        private void HitGround(Collision c)
        {
            HitGround();
        }

        private void HitGround()
        {
            Debug.Log($"Hit Ground!");
            float force = subject.movement.cachedSpeed * subject.rigidbody.mass;
            if (force > MINIMUM_FORCE)
            {
                Vector3 point = subject.transform.position - (Vector3.up * subject.transform.localScale.x * 0.5f);
                PoundGround(point);
            }

            StopEffect();
        }

        private void HitMarble(Collision c, Marble m)
        {
            if (c.gameObject.layer == _toDamageMask)
            {
                float force = c.impulse.magnitude;
                if (force > MINIMUM_FORCE)
                {
                    PoundMarble(m, force);
                }
            }

            StopEffect();
        }

        public float GetDamageRadius()
        {
            return subject.movement.cachedSpeed * 0.15f;
        }
    }


}

