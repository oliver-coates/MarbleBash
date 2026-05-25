using KahuInteractive.HassleFreeConfig;
using UnityEngine;

namespace MarbleBash
{

    /// <summary>
    /// Class responsible for keeping track of a marble's Mutable Stats , i.e move speed, max health, no. of air jumps etc
    /// These values are changed by three sources:
    /// - Core stats (As in agility effects movespeed)
    /// - Status Effects (As in a speed boost or slowdown)
    /// - Upgrades 
    /// </summary>
    public class MarbleStats : MarbleSubComponent
    {
        #region Mutable Stats
        private MutableStat[] _allMutableStats;
        
        // MASS:
        public MutableStat marbleSize;
        public MutableStat rigidbodyMass;
        public MutableStat maxHealth;

        // AGILITY:
        public MutableStat movementSpeed;
        public MutableStat jumpHeight;

        // RECHARGE:
        public MutableStat energyRechargeRate;
        public MutableStat abilityCooldownMultiplier;

        // BLOCK:
        public MutableStat maxShield;
        public MutableStat blockChance;

        // LUCK:
        public MutableStat criticalChance;
        public MutableStat lootRarity;

        // ENERGY:
        public MutableStat maxEnergy;
        public MutableStat shieldRechargeRate;
        
        // ADDITIONAL:
        public MutableStat rigidbodyDrag;

        #endregion

        protected override void Initialise()
        {
            SetupStats();
        }

        private void SetupStats()
        {
            MarbleLevel l = _marble.level;

            #region Mass
            // Mass:
            marbleSize = new MutableStat(
                1f, 
                l.mass,
                Configuration.Read("mass_marble_size_per_level")
            );
            rigidbodyMass = new MutableStat(
                1f,
                l.mass,
                Configuration.Read("mass_mass_per_level")
            );
            maxHealth = new MutableStat(
                1f,
                l.mass,
                Configuration.Read("mass_maxhealth_per_level")
            );
            #endregion

            #region Agility
            // Agility:
            movementSpeed = new MutableStat(
                1f, 
                l.agility,
                Configuration.Read("agility_movespeed_per_level")
            );
            jumpHeight = new MutableStat(
                1f,
                l.agility,
                Configuration.Read("agility_movespeed_per_level"));
            #endregion

            #region Recharge
            // Recharge:
            energyRechargeRate = new MutableStat(
                1f,
                l.recharge,
                Configuration.Read("recharge_recharge_rate_per_level")
            );
            abilityCooldownMultiplier = new MutableStat(
                1f,
                l.recharge,
                Configuration.Read("recharge_cooldown_modifier_per_level")
            );
            #endregion

            #region Block
            // Block:
            maxShield = new MutableStat(
                1f,
                l.block,
                Configuration.Read("block_max_shield_per_level")
            );
            blockChance = new MutableStat(
                1f,
                l.block,
                Configuration.Read("block_block_chance_per_level")
            );
            #endregion

            #region Luck
            // Luck:
            criticalChance = new MutableStat(
                1f,
                l.luck,
                Configuration.Read("luck_critical_chance_per_level")
            );
            lootRarity = new MutableStat(
                1f,
                l.luck,
                Configuration.Read("luck_loot_rarity_per_level")
            );
            #endregion

            #region Energy
            // Energy:
            maxEnergy = new MutableStat(
                1f,
                l.energy,
                Configuration.Read("energy_max_energy_per_level")
            );
            shieldRechargeRate = new MutableStat(
                1f,
                l.energy,
                Configuration.Read("energy_shield_recharge_per_level")
            );
            #endregion
        
            // Additional:
            rigidbodyDrag = new MutableStat(0.25f);
            
            #region Collate Mutable Stats
            _allMutableStats = new MutableStat[]
            {
               marbleSize, rigidbodyMass, maxHealth,
               movementSpeed, rigidbodyDrag,
               energyRechargeRate, abilityCooldownMultiplier,
               maxShield, blockChance,
               criticalChance, lootRarity,
               maxEnergy, shieldRechargeRate  
            };
            #endregion
        }
    
        internal void RecalulcateAllStats()
        {
            foreach (MutableStat stat in _allMutableStats)
            {
                stat.RecalculateValue();
            }
        }
        
    }


}

