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
    public class BuffStacksRenews : Buff, IBuff
    {
        private readonly Game _game;

        public BuffStacksRenews(
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
            IsRootBuff = true;
        }

        public override void OnAddBuff()
        {
            IsRootBuff = true;
            TargetUnit.AddRootBuffInstance(this);

            TargetUnit.AddToBuffList(this);

            if (!IsHidden)
            {
                _game.PacketNotifier.NotifyNPC_BuffAdd2(this, Duration, TimeElapsed);
            }
            
            ActivateBuff();
        }
        public override void OnNewBuff(IBuff b)
        {
            // Don't need the newly added buff instance as we already have a parent who we can add stacks to.
            TargetUnit.RemoveBuffSlot(b);

            // Refresh the time of the parent buff and adds a stack if Max Stacks wasn't reached.
            ResetTimeElapsed();

            if (IncrementStackCount())
            {
                ActivateBuff();
            }

            if (!IsHidden)
            {
                if (BuffType == BuffType.COUNTER)
                {
                    //_game.PacketNotifier .PacketNotifier.NotifyNPC_BuffUpdateNumCounter(this);
                }
                else
                {
                    //_game.PacketNotifier.NotifyNPC_BuffUpdateCount(this, Duration, TimeElapsed);
                }
            }
            // TODO: Unload and reload all data of buff script here.
        }

        public override void OnRemoveBuff(BuffRemovalSource removalSource = BuffRemovalSource.Timeout)
        {
            if (!IsRootBuff)
            {
                RootBuff.OnRemoveBuff();
                return;
            }

            if (StackCount <= 1)
            {
                DeactivateBuff();
                TargetUnit.RemoveRootBuffInstance(this);
                TargetUnit.RemoveBuffSlot(this);
                if (!IsHidden)
                {
                    _game.PacketNotifier.NotifyNPC_BuffRemove2(this);
                }
                return;
            }

            DecrementStackCount();

            if (!IsHidden)
            {
                _game.PacketNotifier.NotifyNPC_BuffUpdateCount(this, Duration - TimeElapsed, TimeElapsed);
            }
        }
    }
}
