using TMPro;
using UnityEngine;

namespace MarbleBash.StatusEffects
{
    [System.Serializable]
    public class Pounding : StatusEffect
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
            DamageManager.ApplyDamage(subject, toPound, force * 1.5f);
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
                    DamageManager.ApplyDamage(subject, hitMarble, damage);
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

