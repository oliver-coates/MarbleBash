using System;
using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    public class PlayerDamageInflictor : MonoBehaviour
    {
        public void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                EnemyInstance enemy = collision.collider.GetComponentSafe<EnemyInstance>();
                
                DamageManager.HandleCollisionWithEnemy(collision, enemy);
            }
        }


    }



}

