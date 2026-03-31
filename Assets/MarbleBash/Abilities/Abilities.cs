using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MarbleBash.Abilities
{
    
    public class Abilities : MonoBehaviour
    {
        [Header("Equipped Abilities:")]
        [SerializeField] private Ability[] _abilities;


        private void Awake()
        {
            _abilities = new Ability[4];
        }

        internal void AttemptActivateAbility(int index)
        {
            Ability ability = _abilities[index];

            // Ensure there is an ability in this slot.
            if (ability == null)
            {
                return;
            }

            // Attempt activation
            ability.AttemptActivate();
        }

        public void EquipAbility(Ability ability, int slot)
        {
            if (slot > 3)
            {
                Debug.LogError($"Ability slots are in the range 0-3, not the given: {slot}");
                return;
            }

            _abilities[slot] = ability;
        }

        private void Update()
        {
            for (int abilityIndex = 0; abilityIndex < 4; abilityIndex++)
            {
                if (_abilities[abilityIndex] != null)
                {
                    _abilities[abilityIndex].Tick();
                }
            }
        }
    }
}
