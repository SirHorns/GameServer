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
    public class BuffStacksContinue : Buff
    {
        private readonly Game _game;
        public List<IBuff> StackList;

        public BuffStacksContinue(
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
            StackList = new List<IBuff>();
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

            // If we've hit the max stacks count for this buff add type
            if (StackCount >= MaxStacks)
            {
                ResetTimeElapsed();

                if (!IsHidden)
                {
                    // If the buff is a counter buff (ex: Nasus Q stacks), then use a packet specialized for big buff stack counts (int.MaxValue).
                    if (BuffType == BuffType.COUNTER)
                    {
                        _game.PacketNotifier.NotifyNPC_BuffUpdateNumCounter(this);
                    }
                    // Otherwise, use the normal buff stack (254) update (usually just adds one to the number on the icon and refreshes the time of the icon).
                    else
                    {
                        _game.PacketNotifier.NotifyNPC_BuffUpdateCount(this, Duration, TimeElapsed);
                    }
                }

                return;
            }

            // Increment the number of stacks on the parent buff, which is the buff instance which is used for packets.
            IncrementStackCount();

            if (!IsHidden)
            {
                _game.PacketNotifier.NotifyNPC_BuffUpdateCount(this, Duration - TimeElapsed, TimeElapsed);
            }
        }

        public override void OnRemoveBuff(BuffRemovalSource removalSource = BuffRemovalSource.Timeout)
        {
            if (!IsRootBuff)
            {
                RootBuff.OnRemoveBuff();
                return;
            }

            if(StackCount <= 1)
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
            ResetTimeElapsed();

            if (!IsHidden)
            {
                // If the buff is a counter buff (ex: Nasus Q stacks), then use a packet specialized for big buff stack counts (int.MaxValue).
                if (BuffType == BuffType.COUNTER)
                {
                    _game.PacketNotifier.NotifyNPC_BuffUpdateNumCounter(this);
                }
                // Otherwise, use the normal buff stack (254) update (usually just adds one to the number on the icon and refreshes the time of the icon).
                else
                {
                    _game.PacketNotifier.NotifyNPC_BuffUpdateCount(this, Duration, TimeElapsed);
                }
            }

        }
    }
}
