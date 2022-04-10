﻿using System;
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
    public class BuffStacksRenew : Buff, IBuff
    {
        protected readonly ILog Logger;
        public BuffStacksRenew(
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
        }

        public bool IncramentStacks()
        {
            if (StackCount < MaxStacks)
            {
                StackCount++;
                return false;
            }
            return true;
        }

        public bool DecramentStacks()
        {
            if (StackCount > 0)
            {
                StackCount++;
                return true;
            }
            return false;
        }

        public override void AddBuff()
        {
            if(IncramentStacks())
            {
                ActivateBuff();
            }
            
            ResetTimeElapsed();
        }
        public override void RemoveBuff()
        {
            if(DecramentStacks())
            {
                DeactivateBuff();
            }
        }
    }
}
