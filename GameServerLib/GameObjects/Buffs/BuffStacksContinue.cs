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
    public class BuffStacksContinue : Buff, IBuff
    {
        protected readonly ILog Logger;
        private IObjAiBase _owner;
        public BuffStacksContinue(
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
            _owner = from;
        }

        public override void AddBuff()
        {
            _owner.GetBuffsWithName(this.Name).ForEach(buff =>
           {
               if(buff != this)
               {
                   ResetTimeElapsed();
               }
           });

            IncrementStackCount();
        }
        public override void RemoveBuff()
        {
            ResetTimeElapsed();
            DecrementStackCount();
            DeactivateBuff();
        }
    }
}
