using System;
using UnityEngine;

namespace MarbleBash
{

    public class MarbleCollisionHandler : MarbleSubComponent
    {
        // On collision with anything:
        public event Action<Collision> OnCollisionAll;
        public static event Action<Marble, Collision> OnCollisionAllGlobal;

        // On collision with another marble.
        // Note that enemies will only trigger this collision event when colliding against the player.
        public event Action<Collision, Marble> OnCollisionMarble;

        // On collision with anything not the player:
        public event Action<Collision> OnCollisionGround;
        public static event Action<Marble, Collision> OnCollisionGroundGlobal;

        // The damage event listener can be overriden with the assigndamagelistener method.
        // This will redirect damage events away from the default listener.
        // Note that this can only be done to the player, enemies don't cause damage events.
        private Action<Collision, EnemyInstance> _damageEventListenerOverride;


        protected override void Initialise()
        {
        }
    
        public void OnCollisionEnter(Collision collision)
        {
            HandleGeneralCollision(collision);

            if (collision.collider.CompareTag("Untagged"))
            {
                HandleGroundCollision(collision);
            }
            else
            {
                HandleNonGroundCollision(collision);
            }

        }

        private void HandleNonGroundCollision(Collision collision)
        {
            if (_marble.isPlayer)
            {
                if (collision.collider.CompareTag("Enemy"))
                {
                    HandleCollisionWithEnemy(collision);
                }
            }
            else
            {
                if (collision.collider.CompareTag("Player"))
                {
                    HandleCollisionWithPlayer(collision);
                }
            }
        }


        #region Collision Handling

        private void HandleGeneralCollision(Collision collision)
        {
            OnCollisionAll?.Invoke(collision);
            OnCollisionAllGlobal?.Invoke(_marble, collision);
        }

        private void HandleGroundCollision(Collision collision)
        {
            OnCollisionGround?.Invoke(collision);
            OnCollisionGroundGlobal?.Invoke(_marble, collision);
        }

        private void HandleCollisionWithPlayer(Collision collision)
        {
            OnCollisionMarble?.Invoke(collision, Player.instance);
        }

        private void HandleCollisionWithEnemy(Collision collision)
        {
            EnemyInstance enemy = collision.collider.GetComponentSafe<EnemyInstance>();

            OnCollisionMarble?.Invoke(collision, enemy);
            NotifyDamageListener(collision, enemy);
        }

        private void NotifyDamageListener(Collision c, EnemyInstance m)
        {
            if (_damageEventListenerOverride != null)
            {
                _damageEventListenerOverride.Invoke(c, m);
            }
            else
            {
                DamageManager.HandleCollisionWithEnemy(c, m);
            }
        }

        #endregion


        #region Damage Listener Override public methods
        public void AssignDamageListener(Action<Collision, EnemyInstance> newListener)
        {
            if (_marble.isPlayer == false)
            {
                Debug.LogError("Only the player marble can have their damage listener overridden.");
                return;
            }
            _damageEventListenerOverride = newListener;
        }

        public void UnassignDamageListener()
        {
            if (_marble.isPlayer == false)
            {
                Debug.LogError("Only the player marble can have their damage listener overridden.");
                return;
            }
            _damageEventListenerOverride = null;
        }
        #endregion
    }


}

