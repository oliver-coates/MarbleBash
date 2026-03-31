using MarbleBash.Abilities;
using UnityEngine;


public class AbilityJumpstart : MonoBehaviour
{
    public Abilities manager;
    public void Start()
    {
        manager.EquipAbility(new PlayerDash(), 0);
        Debug.Log($"Debug: Dash Equipped in slot 0");
    }
}
