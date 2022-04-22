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
    public class BuffStacksOverlaps : Buff, IBuff
    {
        private readonly Game _game;
        private List<IBuff> StackList;

        public BuffStacksOverlaps(
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
            TargetUnit.AddToBuffList(this);

            ActivateBuff();

            _game.PacketNotifier.NotifyNPC_BuffAdd2(this, Duration, TimeElapsed);   
        }
        public override void OnNewBuff(IBuff b)
        {
            // If we've hit the max stacks count for this buff add type (usually 254 for this BuffAddType).
            if (StackCount >= MaxStacks)
            {

                // Get and remove the oldest buff of the same name so we can free up space for the newly given buff instance.
                var oldestBuff = TargetUnit.GetBuffsWithName(Name)[0];
                oldestBuff.OnRemoveBuff();

                // Move the next oldest buff of the same name into the position of the removed oldest buff.
                var tempbuff = TargetUnit.GetBuffsWithName(Name);

                TargetUnit.GetBuffs()[oldestBuff.Slot] = tempbuff[0];
                TargetUnit.AddToBuffList(b);

                if (!IsHidden)
                {
                    // If the buff is a counter buff (ex: Nasus Q stacks), then use a packet specialized for big buff stack counts (int.MaxValue).
                    if (this.BuffType == BuffType.COUNTER)
                    {
                        _game.PacketNotifier.NotifyNPC_BuffUpdateNumCounter(this);
                    }
                    // Otherwise, use the normal buff stack (254) update (usually just adds one to the number on the icon and refreshes the time of the icon).
                    else
                    {
                        _game.PacketNotifier.NotifyNPC_BuffUpdateCount(b, b.Duration, b.TimeElapsed);
                    }
                }
                b.ActivateBuff();

                return;
            }

            // If we haven't hit the max stack count (usually 254).
            TargetUnit.AddToBuffList(b);

            // Increment the number of stacks on the parent buff, which is the buff instance which is used for packets.
            IncrementStackCount();

            // Increment the number of stacks on every buff of the same name (so if any of them become the parent, there is no problem).
            TargetUnit.GetBuffsWithName(Name).ForEach(buff => buff.SetStacks(StackCount));

            if (!IsHidden)
            {
                if (b.BuffType == BuffType.COUNTER)
                {
                    _game.PacketNotifier.NotifyNPC_BuffUpdateNumCounter(this);
                }
                else
                {
                    _game.PacketNotifier.NotifyNPC_BuffUpdateCount(b, b.Duration, b.TimeElapsed);
                }
            }
            b.ActivateBuff();
        }

        public override void OnRemoveBuff(BuffRemovalSource removalSource = BuffRemovalSource.Timeout)
        {
            DeactivateBuff();

            TargetUnit.RemoveFromBuffList(this);
            //TargetUnit.RemoveBuffSlot(this);

            if (!IsHidden)
            {
                _game.PacketNotifier.NotifyNPC_BuffRemove2(this);
            }
        }
    }
}
