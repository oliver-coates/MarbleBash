using System;
using System.Collections.Generic;
using MarbleBash.Enemy;
using UnityEngine;

namespace MarbleBash.Upgrades
{

    public class MarbleUpgrades : MarbleSubComponent
    {
        private List<MarbleUpgrade> _upgrades;

        protected override void Initialise()
        {
            _upgrades = new ();
        }

        public void Add(MarbleUpgrade upgrade)
        {
            _upgrades.Add(upgrade);

            foreach (MarbleUpgradeElement element in upgrade.elements)
            {
                MutableStat stat = GetStatFromName(element.name);

                MutableStatModifier modifier = new MutableStatModifier(MutableStatModifier.Source.Upgrade, element.value, 1f);

                stat.AddModifier(modifier);
            }
        }

        private MutableStat GetStatFromName(string name)
        {
            MarbleStats s = _marble.stats;
            switch (name)
            {
                case "jump_height":
                    return s.jumpHeight;
                
                case "move_speed":
                    return s.movementSpeed;

                default:
                    Debug.LogError($"Unhandled stat name '{name}'");
                    return null;
            }
        }

        internal void SetupUpgradesFromEnemyType(EnemyType type)
        {
            foreach (MarbleUpgrade upgrade in type.upgrades)
            {
                Add(upgrade);
            }
        }
    }


}

