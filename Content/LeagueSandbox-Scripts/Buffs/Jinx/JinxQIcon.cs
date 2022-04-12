using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using LeagueSandbox.GameServer.GameObjects.Stats;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Domain;


using System;

//*=========================================
/*
 * ValkyrieHorns
 * Lastupdated: 4/9/2022
 * 
 * TODOS:
 * 
 * Known Issues:
*/
//*========================================

namespace Buffs
{
    class JinxQIcon : IBuffGameScript
    {
        public IBuffScriptMetaData BuffMetaData { get; set; } = new BuffScriptMetaData
        {
            BuffType = BuffType.COMBAT_ENCHANCER,
            BuffAddType = BuffAddType.REPLACE_EXISTING,
            MaxStacks = 1,
        };

        public IStatsModifier StatsModifier { get; private set; } = new StatsModifier();

        private IObjAiBase _owner;
        
        public void OnActivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
            _owner = ownerSpell.CastInfo.Owner;

            _owner.SetSpellIcon("JinxQ", 0, 1, false);

            //_owner.SetAutoAttackSpell("JinxBasicAttack", false);
        }

        public void OnDeactivate(IAttackableUnit unit, IBuff buff, ISpell ownerSpell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
