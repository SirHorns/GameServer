using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;

namespace GameServerLib.GameObjects.AttackableUnits
{
    class DamageData : IDamageData
    {
        /// <summary>
        /// Whether or not the damage came from an Auto-Attack or a Spell
        /// </summary>
        public bool IsAutoAttack { get; set; }
        /// <summary>
        /// Unit that received the damage.
        /// </summary>
        public IAttackableUnit Target { get; set; }
        /// <summary>
        /// Unit that inflicted the damage.
        /// </summary>
        public IAttackableUnit Attacker { get; set; }
        /// <summary>
        /// Type of damage received.
        /// </summary>
        public DamageType DamageType { get; set; }
        /// <summary>
        /// Source of the damage.
        /// </summary>
        public DamageSource DamageSource { get; set; }
        /// <summary>
        /// The raw amount of damage to be inflicted (Pre-mitigated damage)
        /// </summary>
        public float Damage { get; set; }
        /// <summary>
        /// Mitigated amount of damage (after being reduced by armor/MR stats) 
        /// </summary>
        public float PostMitigationDamage { get; set; }
        
        //Experimental Shield Data Additions
        /// <summary>
        /// Total shielded damage
        /// </summary>
        public float totalDamageShielded { get; set; }
        /// <summary>
        /// Total damage blocked via Standard Shields
        /// </summary>
        public float damageBlockedByStandardShields { get; set; }
        /// <summary>
        /// Total damage blocked via PhysicalDamage Shields
        /// </summary>
        public float damageBlockedByPhysicalDamageShields { get; set; }
        /// <summary>
        /// Total damage blocked via MagicDamage Shields
        /// </summary>
        public float damageBlockedByMagicDamageShields { get; set; }
        /// <summary>
        /// Total damage blocked via TrueDamage Shields
        /// </summary>
        public float damageBlockedByTrueDamageShields { get; set; }
    
        /// <summary>
        /// Amount of damage shielded
        /// </summary>
        public float GetDamageShielded() {
            // not specifying a shield type --> returns damage shielded by ALL shield types
            // 
            // note however that due to only allowing one damage type per instance, this will always be equal to "standard shield block + type shield block"

            return totalDamageShielded;
        }

        /// <summary>
        /// Amount of damage shielded
        /// </summary>
        public float GetDamageShielded(DamageShieldType shieldType) {
            switch(shieldType) {
                default:
                    throw new System.Exception("Unhandled shield type " + shieldType);
                case DamageShieldType.STANDARD_SHIELD:
                    return damageBlockedByStandardShields;
                case DamageShieldType.PHYSICAL_DAMAGE_SHIELD:
                    return damageBlockedByPhysicalDamageShields;
                case DamageShieldType.MAGIC_DAMAGE_SHIELD:
                    return damageBlockedByMagicDamageShields;
                case DamageShieldType.TRUE_DAMAGE_SHIELD:
                    return damageBlockedByTrueDamageShields;
            }
        }
    }
}