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

//*=========================================
/*
 * ValkyrieHorns
 * Lastupdated: 3/20/2022
 * 
 * TODOS:
 * 
 * Known Issues:
 * Waiting for shields to be implemented in LeagueSandbox Gameserver.
*/
//*========================================

namespace Buffs
{
    class OrianaRedactShield : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.SPELL_SHIELD,
            BuffAddType = BuffAddType.RENEW_EXISTING,
            MaxStacks = 1
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier ()
        {
        };

        float _remainingShield;
        private IBuff _thisBuff;
        private IAttackableUnit _target;
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _thisBuff = buff;
            _target = unit;

            _remainingShield = 0;
            var spellLevel = ownerSpell.CastInfo.SpellLevel - 1;
            var shieldBase = new[] { 80, 120, 160, 200, 240 }[spellLevel];
            var finalShield = shieldBase + (.4f * ownerSpell.CastInfo.Owner.Stats.AbilityPower.Total);

            _remainingShield = finalShield;

            StatsModifier.NormalShieldPoints.FlatBonus = 600f;
            unit.ModifyShield(600f, ShieldType.SHIELD_PHYSICAL, true);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            ModifyShield(unit, -1000, true, true, false);
        }

        public void OnPreAttack(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
