using System;
using System.Collections;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    /// <summary>
    /// Handles all collisions against the player marble.
    /// </summary>
    public class PlayerCollisionHandler : MonoBehaviour
    {
        public static event Action<Collision> OnCollisionAll;

        public static event Action<Collision, EnemyInstance> OnCollisionMarble;

        public static event Action<Collision> OnCollisionGround;


        private static Action<Collision, EnemyInstance> _damageEventListener;



        public void OnCollisionEnter(Collision collision)
        {
            OnCollisionAll?.Invoke(collision);

            if (collision.collider.CompareTag("Untagged"))
            {             
                OnCollisionGround?.Invoke(collision);
            }
            if (collision.collider.CompareTag("Enemy"))
            {
                EnemyInstance enemy = collision.collider.GetComponentSafe<EnemyInstance>();
                
                OnCollisionMarble?.Invoke(collision, enemy);
                NotifyDamageListener(collision, enemy);
            }
        }

        private void NotifyDamageListener(Collision c, EnemyInstance m)
        {
            if (_damageEventListener != null)
            {
                _damageEventListener.Invoke(c, m);
            }
            else
            {
                DamageManager.HandleCollisionWithEnemy(c, m);
            }
        }

        public static void AssignDamageListener(Action<Collision, EnemyInstance> newListener)
        {
            _damageEventListener = newListener;
        }

        public static void UnassignDamageListener()
        {
            _damageEventListener = null;
        }


    }



}

