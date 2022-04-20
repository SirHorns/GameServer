using System;
using System.Collections.Generic;
using System.Text;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using System.Numerics;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using GameServerCore.Domain;
using GameServerLib.GameObjects.AttackableUnits;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Logging;
using log4net;

namespace LeagueSandbox.GameServer.GameObjects
{
    public class Shield : IShield
    {
        private static ILog _logger;
        public float shieldHealthCurrent { get; private set; }
        public float shieldHealthBase { get; private set; }
        public float shieldHealthMax { get; private set; }
        public float ShieldHealthBonus { get { return shieldHealthMax - shieldHealthBase; } }

        public IBuff sourceBuff { get; set; }
        public ShieldType shieldType { get; private set; }
        public bool isTrueShield { get; private set; }

        public bool initializing { get; private set; }

        public Shield(IBuff sourceBuff, float maxShieldHealth, ShieldType shieldType = ShieldType.STANDARD_SHIELD, bool isTrueShield = false)
        {
            _logger = LoggerProvider.GetLogger();
            if (sourceBuff != null)
            {
                Exception e = new Exception("Cannot create a new shield instance on a Debuff that already has a shield instance (you should create a separate Debuff instead");

                _logger.Error(e);
                throw e;
            }

            this.sourceBuff = sourceBuff;
            this.shieldType = shieldType;
            this.isTrueShield = isTrueShield;

            shieldHealthCurrent = 0;

            initializing = true;

            sourceBuff.ShieldInstance = this;

            shieldHealthBase = maxShieldHealth;  // assigning base health directly (there isn't even a method for it anyways)

            if (maxShieldHealth < 0)
            {
                // negative shields wouldn't make any sense and will trigger an immediately Debuff removal
                maxShieldHealth = 0;
            }

            shieldHealthMax = maxShieldHealth;  // assigning max health directly, NOT via method
            shieldHealthCurrent = maxShieldHealth;
        }

        public void RemoveShield(bool stopShieldFade)
        {
            if (this.shieldHealthCurrent != 0)
            {  // SetShieldHealth() does an auto-remove when setting to zero, so we need to protect from a stack overflow here
                this.SetShieldHealthCurrent(0, null);
            }

            this.sourceBuff.TargetUnit.RemoveShield(this, stopShieldFade);
            this.sourceBuff.ShieldInstance = null;
        }

        public void SetShieldHealthCurrent(float newValue, IDamageData damageInstance)
        {
            float rawNewValue = newValue;  // this is intended to let you know if a shield tried to go over its max value or something

            if (newValue < 0)
            {
                newValue = 0;
            }

            if (newValue > shieldHealthMax)
            {
                newValue = shieldHealthMax;
            }

            float oldShieldHealth = shieldHealthCurrent;
            shieldHealthCurrent = newValue;

            //UnitStat shieldStat = sourceBuff.TargetUnit.GetShieldHealthStat(shieldType);

            float gain = newValue - oldShieldHealth;  // remaining shield if this is a reapplied buff, zero if this is a new buff
            //shieldStat.CurrentValue += gain;

            // we want an assignment to 0 health to count as a removal/manual break, but we also want to *not* do that immediately on creation until
            // after the shield apply/receive events have gone off, since those might modify the shield health into being nonzero
            if (newValue <= 0 && this.initializing == false)
            {
                this.RemoveShield(false);
            }
        }

        public void SetShieldHealthMax(float newValue, bool applyToCurrentHealth)
        {
            if (newValue < 0)
            {
                newValue = 0;
            }

            float oldMax = shieldHealthMax;
            shieldHealthMax = newValue;

            if (applyToCurrentHealth == true)
            {
                float gain = newValue - oldMax;
                float newCurrent = this.shieldHealthCurrent + gain;
                SetShieldHealthCurrent(newCurrent, null);
            }

            if (shieldHealthCurrent > shieldHealthMax)
            {
                // only applies a cap to current health over max health
                // will still increase current health if the shield is currently full
                SetShieldHealthCurrent(shieldHealthMax, null);
            }
        }

        public void SetShieldHealthBonus(float newValue, bool applyToCurrentHealth)
        {
            float newMax = this.shieldHealthBase + newValue;
            this.SetShieldHealthMax(newMax, applyToCurrentHealth);
        }
    }
}
