using MarbleBash;
using MarbleBash.Abilities;
using UnityEngine;


public class AbilityJumpstart : MonoBehaviour
{
    public void Start()
    {
        Marble subject = this.GetComponentSafe<Marble>();
        Abilities abilities = this.GetComponentSafe<Abilities>();

        abilities.EquipAbility(new Dash(subject), 0);
    }
}
