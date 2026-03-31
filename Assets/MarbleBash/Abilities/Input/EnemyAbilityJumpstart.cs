using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash.Abilities
{
    public class EnemyAbilityJumpstart : MonoBehaviour
    {


        public void Start()
        {
            this.GetComponentSafe<Abilities>().EquipAbility(new EnemyDash(this.GetComponentSafe<EnemyInstance>()), 0);    
        }
    }
}