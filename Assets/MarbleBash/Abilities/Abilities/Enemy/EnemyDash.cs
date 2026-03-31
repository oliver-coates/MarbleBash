using JetBrains.Annotations;
using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash.Abilities
{
    [System.Serializable]
    public class EnemyDash : EnemyAbility
    {
        public EnemyDash(EnemyInstance subject) : base(subject)
        {
            _name = "Dash";
        }


        protected override void Activate()
        {
            Vector3 force = GetDirToPlayer() * 25f;

            _subject.rb.AddForce(force, ForceMode.VelocityChange);
        }

        protected override bool IsAbleToActivate()
        {   
            return GetDistanceToPlayer() < 5f; 
        }

        private Vector3 GetDirToPlayer()
        {
            return (Player.transform.position - _subject.transform.position).normalized;
        }
        
        private float GetDistanceToPlayer()
        {
            return Vector3.Distance(_subject.transform.position, Player.transform.position);
        }
    }
}