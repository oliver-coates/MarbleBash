using System;
using KahuInteractive.HassleFreeConfig;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public class BombExplosion : MonoBehaviour
    {
        // Configuration:
        private float _knockbackMultiplier = 1f;
        private float _knockbackUpAmount = 2f;
        private float _velocityNeutralisation = 0.25f;

        public void Initialise(float radius, float damage, Marble caster)
        {
            MasksConfig masks = Configuration.Get<MasksConfig>();

            float diameter = radius * 2f;
            transform.localScale = new Vector3(diameter, diameter, diameter);

            MarbleHit[] marblesToDamage = GetMarblesInRadius(radius, masks.allMarbles);
            
            DamageHitMarbles(radius, damage, caster, marblesToDamage);

            Destroy(gameObject, 2f);
        }

        private void DamageHitMarbles(float radius, float damage, Marble caster, MarbleHit[] marblesToDamage)
        {
            foreach (MarbleHit hit in marblesToDamage)
            {
                float distanceLerp = 1f - (hit.distance / radius);
                float distanceFactor = Mathf.Clamp(distanceLerp, 0.15f, 1f);

                // Neutralise their velocity:
                hit.marble.rigidbody.linearVelocity *= (1f - _velocityNeutralisation);

                // Apply stun:
                hit.marble.transform.position += Vector3.up * 0.1f; // Lift slighty off the ground so the stun doesn't shut off instantly
                hit.marble.statusEffects.AddEffect<AirStunned>(25f);

                // Damage Marble:
                DamageEvent damageEvent = new DamageEvent(caster, hit.marble)
                {
                    amount = damage * distanceFactor,
                    knockbackDirection = GetKnockbackDirection(hit.marble.transform.position),
                    knockbackAmount = damage * distanceFactor * _knockbackMultiplier
                };

                hit.marble.health.TakeDamage(damageEvent);
            }
        }

        private Vector3 GetKnockbackDirection(Vector3 marblePosition)
        {
            Vector3 dir2d = (marblePosition - transform.position).ShearTo2D();

            Vector3 upDirection = Vector3.up * _knockbackUpAmount;

            return (dir2d + upDirection).normalized;
        }

        private MarbleHit[] GetMarblesInRadius(float radius, LayerMask allMarbles)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, allMarbles);

            MarbleHit[] hits = new MarbleHit[hitColliders.Length];
            
            // Convert all hit colliders into marble hit objects
            for (int hitIndex = 0; hitIndex < hitColliders.Length; hitIndex++)
            {
                MarbleHit newHit = new MarbleHit();
                Collider collider = hitColliders[hitIndex];

                newHit.marble = collider.gameObject.GetComponent<Marble>();
                newHit.distance = Vector3.Distance(transform.position, collider.transform.position);

                hits[hitIndex] = newHit;
            }
        
            return hits;
        }

        private class MarbleHit
        {
            public Marble marble;
            public float distance;
        }
    }

}

