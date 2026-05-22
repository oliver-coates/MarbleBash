using MarbleBash;
using MarbleBash.Abilities;
using UnityEngine;


public class EnemyAbilityJumpstart : MonoBehaviour
{
    public void Start()
    {
        Marble subject = this.GetComponentSafe<Marble>();
        AbilityController abilities = this.GetComponentSafe<AbilityController>();

        abilities.EquipAbility(new Dash(subject), 0);
    }
}
