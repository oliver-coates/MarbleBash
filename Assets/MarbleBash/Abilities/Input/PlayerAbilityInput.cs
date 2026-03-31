using UnityEngine;
using UnityEngine.InputSystem;

namespace MarbleBash.Abilities
{
    public class PlayerAbilityInput : MonoBehaviour
    {
        [SerializeField] private Abilities _target;

        
        [Header("Input:")]
        public InputActionAsset inputActions;

        private void SetupInput()
        {
            InputActionMap map = inputActions.FindActionMap("Player");

            map.FindAction("Ability 1").performed += (InputAction.CallbackContext c) => { _target.AttemptActivateAbility(0); };
            map.FindAction("Ability 2").performed += (InputAction.CallbackContext c) => { _target.AttemptActivateAbility(1); };
            map.FindAction("Ability 3").performed += (InputAction.CallbackContext c) => { _target.AttemptActivateAbility(2); };
            map.FindAction("Ability 4").performed += (InputAction.CallbackContext c) => { _target.AttemptActivateAbility(3); };
        }

        private void Awake()
        {
            SetupInput();
        }   
    }
    
}
