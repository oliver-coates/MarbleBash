using TMPro;
using UnityEngine;

namespace MarbleBash.Abilities
{
    [System.Serializable]
    public class Pounding : AbilityEffect
    {
        private MarbleCollisionEffectManager _collisionManager;
        public Pounding() : base()
        {
            _duration = 10f;
        }

        protected override void Start()
        {
            _collisionManager = Player.instance.gameObject.GetComponent<MarbleCollisionEffectManager>(); 
            
            _collisionManager.OnCollisionOccured += CollisionOccured; 
        }

        private void CollisionOccured(Collision c)
        {
            float force = c.impulse.magnitude;
            if (force > 5f)
            {
                if (c.transform.CompareTag("Enemy"))
                {
                    Marble toPound = c.gameObject.GetComponent<Marble>();
                    PoundMarble(toPound, force);
                }
                else
                {
                    PoundGround(subject.transform.position, force);                
                }
            }

            StopEffect();
        }

        private void PoundMarble(Marble toPound, float force)
        {
            // Randomise knockback direction - It doesn't really matter here as the 
            // physics engine is going to freak out from the player pounding onto the enemy.
            Vector3 knockbackDir = new Vector3(Random.Range(0,1f), 0, Random.Range(0, 1f)).normalized;
            
            DamageManager.ApplyDamage(subject, toPound, force * 1.5f, knockbackDir);

            Vector3 velocity = subject.rigidbody.linearVelocity;
            velocity.y = -subject.cachedVelocity.y * 0.5f;

            subject.rigidbody.linearVelocity = velocity;
        }

        private void PoundGround(Vector3 pos, float force)
        {
            float size = force * 0.33f;

            Collider[] hits = Physics.OverlapSphere(pos, size, LayerMask.GetMask("Default"));
            foreach (Collider hit in hits)
            {
                if (hit.tag is "Enemy" or "Player")
                {
                    Marble hitMarble = hit.gameObject.GetComponent<Marble>();
                    float distanceL = Vector3.Distance(pos, hitMarble.transform.position) / size;

                    float damage = force * distanceL;

                    Vector3 knockbackDir = hitMarble.transform.position - subject.transform.position;
                    knockbackDir.y = 3f;

                    DamageManager.ApplyDamage(subject, hitMarble, damage, knockbackDir, 1.25f);
                }
            }

            VisualEffectManager.CreateEffect("Ground Pound", pos, size);
        }

        protected override void Update() {}

        protected override void Finished()
        {
            _collisionManager.OnCollisionOccured -= CollisionOccured;
        }

    }


}

