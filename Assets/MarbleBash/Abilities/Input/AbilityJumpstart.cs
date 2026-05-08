using MarbleBash;
using MarbleBash.Abilities;
using UnityEngine;


public class AbilityJumpstart : MonoBehaviour
{
    #region Initalisation & Destruction

    private void Awake()
    {
        GameController.OnInitialisePlayerSecondary += Setup;
    }

    private void OnDestroy()
    {
        GameController.OnInitialisePlayerSecondary -= Setup;
    }

    #endregion

    public void Setup()
    {
        Marble subject = this.GetComponentSafe<Marble>();
        AbilityController abilities = this.GetComponentSafe<AbilityController>();

        abilities.EquipAbility(new Dash(subject), 0);
        abilities.EquipAbility(new GroundPound(subject), 1);
        abilities.EquipAbility(new Charge(subject), 2);
    }
}
