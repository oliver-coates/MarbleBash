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
            _duration = 2f;
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
                Explode(subject.transform.position, force);
            }

            StopEffect();
        }

        private void Explode(Vector3 pos, float force)
        {
            Debug.Log("BOOOOOOOOOOM!");
        }

        protected override void Update() {}

        protected override void Finished()
        {
            _collisionManager.OnCollisionOccured -= CollisionOccured;
        }

    }


}

