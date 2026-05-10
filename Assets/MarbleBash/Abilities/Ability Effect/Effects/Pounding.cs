using KahuInteractive.VisualFX;
using TMPro;
using UnityEngine;

namespace MarbleBash.Abilities
{
    [System.Serializable]
    public class Pounding : AbilityEffect
    {
        public Pounding() : base()
        {
            _duration = 10f;
        }

        protected override void Start()
        {
            PlayerCollisionHandler.OnCollisionGround += HitGround;
            PlayerCollisionHandler.AssignDamageListener(HitMarble);
        }

        private void PoundMarble(Marble toPound, float force)
        {
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

        private void PoundGround(Vector3 pos, float force)
        {
            float size = force * 0.33f;

            Collider[] hits = Physics.OverlapSphere(pos, size, LayerMask.GetMask("Default"));
            foreach (Collider hit in hits)
            {
                if (hit.tag is "Enemy" or "Player")
                {
                    Marble hitMarble = hit.gameObject.GetComponent<Marble>();
                    float distanceL = 1f - (Vector3.Distance(pos, hitMarble.transform.position) / size);

                    float damage = force * distanceL;

                    Vector3 knockbackDir = hitMarble.transform.position - subject.transform.position;
                    knockbackDir.y = 3f;

                    DamageManager.ApplyDamage(subject, hitMarble, damage, knockbackDir, 1.25f);
                }
            }
            
            VFX.Play(new OneShotEffectData("Ground Pound", pos, Quaternion.identity, size));
        }

        protected override void Update()
        {
            Vector3 force = Vector3.down * 1000 * Time.deltaTime;
            subject.rigidbody.AddForce(force);
        }

        protected override void Finished()
        {
            PlayerCollisionHandler.OnCollisionGround -= HitGround;
            PlayerCollisionHandler.UnassignDamageListener();
        }

        private void HitGround(Collision c)
        {
            float force = c.impulse.magnitude;
            if (force > 5f)
            {
                PoundGround(c.contacts[0].point, force);
            }

            StopEffect();
        }

        private void HitMarble(Collision c, Marble m)
        {
            float force = c.impulse.magnitude;
            if (force > 5f)
            {
                PoundMarble(m, force);
            }

            StopEffect();
        }

    }


}

