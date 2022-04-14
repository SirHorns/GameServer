using System;
using GameServerCore;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using LeagueSandbox.GameServer.Scripting.CSharp;
using LeagueSandbox.GameServer.GameObjects.Other;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using System.Collections.Generic;
using GameServerCore.Domain;
using LeagueSandbox.GameServer.GameObjects.Stats;

using LeagueSandbox.GameServer.Logging;
using log4net;

namespace LeagueSandbox.GameServer.GameObjects
{
    public class BuffStacksDecays : Buff, IBuff
    {
        protected readonly ILog Logger;
        private IAttackableUnit _unit;
        private IBuff BuffList;
        public BuffStacksDecays(
            Game game,
            string buffName,
            float duration,
            int stacks,
            ISpell originSpell,
            IAttackableUnit onto,
            IObjAiBase from,
            bool infiniteDuration = false
            ) : base(game, buffName, duration, stacks, originSpell, onto, from, infiniteDuration)
        {
            Logger = LoggerProvider.GetLogger();
            _unit = onto;
        }

        private bool IsParentBuff = false;
        public override void AddBuff()
        {
            if(StackCount < MaxStacks)
            {
                IncrementStackCount();
            }

            ResetTimeElapsed();
        }
        public override void RemoveBuff()
        {
            DecrementStackCount();
            ResetTimeElapsed();
        }
    }
}
