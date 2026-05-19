using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MarbleBash.Abilities
{
    public class AbilityController : MonoBehaviour
    {
        [Header("Equipped Abilities:")]
        [SerializeField] private Ability[] _abilities;
        private Dictionary<string, Ability> _nameToAbilityDict;


        private void Awake()
        {
            _nameToAbilityDict = new();
            _abilities = new Ability[4];
        }

        public bool IsAbilityAbleToActivate(string name)
        {
            return _nameToAbilityDict[name].CheckIsAbleToActivate();
        }

        public bool AttemptActivateAbility(string name)
        {
            return _nameToAbilityDict[name].AttemptActivate();
        }

        public bool IsAbilityOnCooldown(string name)
        {
            return !_nameToAbilityDict[name].CheckIsNotOnCooldown();
        }

        internal bool AttemptActivateAbility(int index)
        {
            Ability ability = _abilities[index];

            // Ensure there is an ability in this slot.
            if (ability == null)
            {
                return false;
            }

            // Attempt activation
            return ability.AttemptActivate();
        }

        public void EquipAbility(Ability ability, int slot)
        {
            if (slot > 3)
            {
                Debug.LogError($"Ability slots are in the range 0-3, not the given: {slot}");
                return;
            }

            _nameToAbilityDict[ability.name] = ability;
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
