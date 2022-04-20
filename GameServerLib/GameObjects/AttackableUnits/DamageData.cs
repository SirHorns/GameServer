using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;

namespace GameServerLib.GameObjects.AttackableUnits
{
    class DamageData : IDamageData
    {
        /// <summary>
        /// Wheter or not the damage came from an auttoatack or a Spell
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
        /// The raw ammount of damage to be inflicted (Pre-mitigated damage)
        /// </summary>
        public float Damage { get; set; }
        /// <summary>
        /// Mitigated ammount of damage (after being reduced by armor/MR stats) 
        /// </summary>
        public float PostMitigationdDamage { get; set; }

        /// <summary>
        /// Total Damage Shielded
        /// </summary>
        public float totalDamageShielded { get; }
        /// <summary>
        /// Damage Shielded by Standard Shields
        /// </summary>
        public float damageBlockedByStandardShields { get; }
        /// <summary>
        /// Damage Shielded by Physical Shields
        /// </summary>
        public float damageBlockedByPhysicalDamageShields { get; }
        /// <summary>
        /// Damage Shielded by Magical Shields
        /// </summary>
        public float damageBlockedByMagicDamageShields { get; }
        /// <summary>
        /// Damage Shielded by True Damage Shields
        /// </summary>
        public float damageBlockedByTrueDamageShields { get; }


        public float GetDamageShielded()
        {
            // not specifying a shield type --> returns damage shielded by ALL shield types
            // 
            // note however that due to only allowing one damage type per instance, this will always be equal to "standard shield block + type shield block"

            return totalDamageShielded;
        }

        public float GetDamageShielded(ShieldType shieldType)
        {
            switch (shieldType)
            {
                default:
                    throw new System.Exception("Unhandled shield type " + shieldType);
                case ShieldType.STANDARD_SHIELD:
                    return damageBlockedByStandardShields;
                case ShieldType.PHYSICAL_SHIELD:
                    return damageBlockedByPhysicalDamageShields;
                case ShieldType.MAGICAL_SHIELD:
                    return damageBlockedByMagicDamageShields;
                case ShieldType.TRUEDAMAGE_SHIELD:
                    return damageBlockedByTrueDamageShields;
            }
        }
    }
}