using System.Numerics;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System;
using GameServerCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerLib.GameObjects.AttackableUnits;
using LeagueSandbox.GameServer.GameObjects;
using LeagueSandbox.GameServer.GameObjects.AttackableUnits.AI;
using LeagueSandbox.GameServer.GameObjects.Spell;
using LeagueSandbox.GameServer.Logging;
using LeagueSandbox.GameServer.Scripting.CSharp;
using log4net;

//*=========================================
/*
 * ValkyrieHorns
 * Lastupdated:
 * 
 * TODOS:
 * 
 * Known Issues:
 * 
*/
//*========================================

namespace Buffs
{
    class EyeOfTheStorm : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SPELL_SHIELD,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1,
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier()
        {
        };

        private IObjAiBase _owner;
        private IBuff _thisBuff;
        private IAttackableUnit _target;

        private float _shieldDecay;
        private IParticle _shieldTowerParticle1;
        private IParticle _shieldTowerParticle2;

        private IParticle _shieldAllyParticle;
        private float FinalSHield;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _owner = ownerSpell.CastInfo.Owner;
            _thisBuff = buff;
            _target = unit;

            var spellLevel = ownerSpell.CastInfo.SpellLevel - 1;

            var baseShield = new[] { 80.0f, 110.0f, 150.0f, 170.0f, 200.0f }[spellLevel];
            var finalShield = baseShield + (.65f * ownerSpell.CastInfo.Owner.Stats.AbilityPower.Total);

            var baseBonusAD = new[] { 10.0f, 17.5f, 25.0f, 32.5f, 40.0f }[spellLevel];
            var finalBonusAD = baseBonusAD + (.10f * ownerSpell.CastInfo.Owner.Stats.AbilityPower.Total);


            BuffStartParticles(unit, buff.Duration);

            ApplyStatModifiers(unit, 1000f, finalBonusAD);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            BuffEndParticles(unit);

            unit.Stats.CurrentHealth = unit.Stats.CurrentHealth;
        }

        float lastTick = 0.0f;
        public void OnUpdate(float diff)
        {
        }

        private void BuffStartParticles(IAttackableUnit unit, float duration)
        {
            /*
             * EyeoftheStorm_Break.troy
             * EyeoftheStorm_buf.troy
             * EyeoftheStorm_Frost_Ally_buf.troy
             * EyeoftheStorm_Frost_Tower_buf.troy
             * EyeoftheStorm_Tower_buf.troy
             */

            if (unit is IBaseTurret || unit is ILaneTurret)
            {
                //_shieldTowerParticle1 = AddParticleTarget(_owner, unit, "EyeoftheStorm_Frost_Tower_buf.troy", unit, 2500f, bone: "pelvis");
                _shieldTowerParticle2 = AddParticleTarget(_owner, unit, "EyeoftheStorm_Tower_buf.troy", unit, duration);
            }
            if (unit is IChampion && unit != _owner)
            {
                _shieldAllyParticle = AddParticleTarget(_owner, unit, "EyeoftheStorm_Frost_Ally_buf", unit, duration, bone: "C_BUFFBONE_GLB_CENTER_LOC");
            }
            if (unit == _owner)
            {
                _shieldAllyParticle = AddParticleTarget(_owner, unit, "EyeoftheStorm_buf", unit, duration, bone: "C_BUFFBONE_GLB_CENTER_LOC");
            }
        }

        private void BuffEndParticles(IAttackableUnit unit)
        {
            RemoveParticle(_shieldTowerParticle1);
            RemoveParticle(_shieldTowerParticle2);
            RemoveParticle(_shieldAllyParticle);
            RemoveParticle(_shieldAllyParticle);

            AddParticleTarget(_owner, unit, "EyeoftheStorm_Break.troy", unit, bone: "C_BUFFBONE_GLB_CENTER_LOC");
        }

        private void ApplyStatModifiers(IAttackableUnit unit, float shieldAmount, float bAD)
        {
            StatsModifier.AttackDamage.FlatBonus = bAD;

            unit.AddStatModifier(StatsModifier);

            IShield shield = new Shield(_thisBuff, shieldAmount);


            unit.AddShield(shield,false);
        }
    }
}
