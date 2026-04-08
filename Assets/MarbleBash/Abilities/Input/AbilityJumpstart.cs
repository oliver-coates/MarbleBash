using MarbleBash;
using MarbleBash.Abilities;
using UnityEngine;


public class AbilityJumpstart : MonoBehaviour
{
    public void Start()
    {
        Marble subject = this.GetComponentSafe<Marble>();

        MarbleBash.Player.abilities.EquipAbility(new Dash(subject), 0);
        Debug.Log($"Debug: Dash Equipped in slot 0");
    }
}
