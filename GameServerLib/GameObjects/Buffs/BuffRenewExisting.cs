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


namespace LeagueSandbox.GameServer.GameObjects
{
    /// <summary>
    /// This Buff Type will just renew itself when a buff of the same type is added to a unit that already has the same buff instance applied.
    /// </summary>
    public class BuffRenewExisting : Buff, IBuff
    {
        private readonly Game _game;

        public BuffRenewExisting(
            Game game, 
            string buffName, 
            float duration, 
            int stacks, 
            ISpell originSpell, 
            IAttackableUnit onto, 
            IObjAiBase from, 
            bool infiniteDuration = false, 
            bool pauseBuffTimer = false
            ) : base(game, buffName, duration, stacks, originSpell, onto, from, infiniteDuration, pauseBuffTimer)
        {
            _game = game;
        }

        public override void OnAddBuff()
        {
            IsRootBuff = true;
            TargetUnit.AddRootBuffInstance(this);
            TargetUnit.AddToBuffList(this);
            

            ActivateBuff();

            _game.PacketNotifier.NotifyNPC_BuffAdd2(this, Duration, TimeElapsed);
        }
        public override void OnNewBuff(IBuff b)
        {
            TargetUnit.RemoveBuffSlot(b);

            ResetTimeElapsed();

            if (!IsHidden)
            {
                _game.PacketNotifier.NotifyNPC_BuffReplace(this);
            }
        }

        public override void OnRemoveBuff(BuffRemovalSource removalSource = BuffRemovalSource.Timeout)
        {
            TargetUnit.RemoveRootBuffInstance(this);

            DeactivateBuff();

            if (!IsHidden)
            {
                _game.PacketNotifier.NotifyNPC_BuffRemove2(this);
            }
        }
    }
}
