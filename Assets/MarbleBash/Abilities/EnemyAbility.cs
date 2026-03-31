using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash.Abilities
{

    public abstract class EnemyAbility : Ability
    {
        [Header("Target:")]
        [SerializeField] protected EnemyInstance _subject;

        public EnemyAbility(EnemyInstance subject) : base()
        {
            _subject = subject;
        }
    }


}

