using MarbleBash.Abilities;
using UnityEngine;


public class AbilityJumpstart : MonoBehaviour
{
    public AbilityManager manager;
    public void Start()
    {
        manager.EquipAbility(new Dash(), 0);
        Debug.Log($"Debug: Dash Equipped in slot 0");
    }
}
