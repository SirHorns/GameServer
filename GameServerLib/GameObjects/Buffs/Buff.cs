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
    public class Buff : Stackable, IBuff
    {
        // Crucial Vars.
        private readonly Game _game;
        private readonly CSharpScriptEngine _scriptEngine;

        // Function Vars.
        private readonly bool _infiniteDuration;
        private bool _removed;
        private bool IsTimerPostponed = true;

        public BuffType BuffType { get; } /// TODO: Add comments to BuffType enum.
        public BuffAddType BuffAddType { get; }
        public BuffRemovalSource BuffRemovalSource { get; private set; }
        public string Name { get; }
        public float Duration { get; }
        public float TimeElapsed { get; private set; }
        public float TimeRemaining { get; private set; }
        public byte Slot { get; private set; }
        public bool IsHidden { get; }
        public ISpell OriginSpell { get; }
        public IObjAiBase SourceUnit { get; }
        public IAttackableUnit TargetUnit { get; }
        
        /// <summary>
        /// Script instance for this buff. Casting to a specific buff class gives access its functions and variables.
        /// </summary>
        public IBuffGameScript BuffScript { get; private set; }
        /// <summary>
        /// All status effects applied by this buff.
        /// </summary>
        public Dictionary<StatusFlags, bool> StatusEffects { get; private set; }
        /// <summary>
        /// Used to update player buff tool tip values.
        /// </summary>
        public IToolTipData ToolTipData { get; protected set; }

        public Buff(Game game, string buffName, float duration, int stacks, ISpell originSpell, IAttackableUnit onto, IObjAiBase from, bool infiniteDuration = false, bool pauseBuffTimer = false)
        {
            if (duration < 0)
            {
                throw new ArgumentException("Error: Duration was set to under 0.");
            }

            IsTimerPostponed = pauseBuffTimer;
            _infiniteDuration = infiniteDuration;
            _game = game;
            _removed = false;
            _scriptEngine = game.ScriptEngine;
            Name = buffName;

            LoadScript();

            BuffRemovalSource = BuffRemovalSource.NotYetRemoved;

            BuffAddType = BuffScript.BuffMetaData.BuffAddType;
            if (BuffAddType == (BuffAddType.STACKS_AND_RENEWS | BuffAddType.STACKS_AND_CONTINUE | BuffAddType.STACKS_AND_OVERLAPS) && BuffScript.BuffMetaData.MaxStacks < 2)
            {
                throw new ArgumentException("Error: Tried to create Stackable Buff, but MaxStacks was less than 2.");
            }

            BuffType = BuffScript.BuffMetaData.BuffType;
            Duration = duration;
            TimeRemaining = duration;
            IsHidden = BuffScript.BuffMetaData.IsHidden;
            if (BuffScript.BuffMetaData.MaxStacks > 254 && BuffType != BuffType.COUNTER)
            {
                MaxStacks = 254;
            }
            else
            {
                MaxStacks = Math.Min(BuffScript.BuffMetaData.MaxStacks, int.MaxValue);
            }
            OriginSpell = originSpell;
            if (onto.HasBuff(Name) && BuffAddType == BuffAddType.STACKS_AND_OVERLAPS)
            {
                // Put parent buff data into children buffs
                StackCount = onto.GetBuffWithName(Name).StackCount;
                Slot = onto.GetBuffWithName(Name).Slot;
            }
            else
            {
                StackCount = stacks;
                Slot = onto.GetNewBuffSlot(this);
            }
            SourceUnit = from;
            TimeElapsed = 0;
            TargetUnit = onto;
            StatusEffects = new Dictionary<StatusFlags, bool>();

            ToolTipData = new ToolTipData(TargetUnit, null, this);
        }

        public bool PauseBuffTimer(bool pause)
        {
            if (!_infiniteDuration)
            {
                return IsTimerPostponed = pause;
            }

            return false;
        }
        public void LoadScript()
        {
            ApiEventManager.RemoveAllListenersForOwner(BuffScript);
            BuffScript = _scriptEngine.CreateObject<IBuffGameScript>("Buffs", Name) ?? new BuffScriptEmpty();
        }

        public void ActivateBuff()
        {
            _removed = false;

            BuffScript.OnActivate(TargetUnit, this, OriginSpell);
        }

        public void DeactivateBuff()
        {
            if (_removed)
            {
                return;
            }

            _removed = true; // To prevent infinite loop with OnDeactivate calling events

            BuffScript.OnDeactivate(TargetUnit, this, OriginSpell);

            if (BuffScript.StatsModifier != null)
            {
                TargetUnit.RemoveStatModifier(BuffScript.StatsModifier);
            }
        }

        public bool Elapsed()
        {
            return _removed;
        }

        public IStatsModifier GetStatsModifier()
        {
            return BuffScript.StatsModifier;
        }

        public void SetStatusEffect(StatusFlags flag, bool enabled)
        {
            // Loop over all possible status flags and assign them individually to the dictionary
            for (int i = 0; i < 30; i++)
            {
                StatusFlags currentFlag = (StatusFlags)(1 << i);

                if (flag.HasFlag(currentFlag))
                {
                    if (StatusEffects.ContainsKey(currentFlag))
                    {
                        StatusEffects[currentFlag] = enabled;
                        continue;
                    }

                    StatusEffects.Add(currentFlag, enabled);
                }
            }
        }

        public void SetToolTipVar<T>(int tipIndex, T value) where T : struct
        {
            ToolTipData.Update(tipIndex, value);

            if (TargetUnit is IChampion champ)
            {
                champ.AddToolTipChange(ToolTipData);
            }
        }

        public bool IsBuffInfinite()
        {
            return _infiniteDuration;
        }

        public bool IsBuffSame(string buffName)
        {
            return buffName == Name;
        }

        public void ResetTimeElapsed()
        {
            TimeElapsed = 0;
            TimeRemaining = Duration;
        }

        public void SetSlot(byte slot)
        {
            Slot = slot;
        }

        public void Update(float diff)
        {
            if (_infiniteDuration)
            {
                return;
            }

            if (!IsTimerPostponed)
            {
                TimeElapsed += diff / 1000.0f;
                TimeRemaining = Duration / TimeElapsed;
                if (Math.Abs(Duration) > Extensions.COMPARE_EPSILON)
                {
                    BuffScript?.OnUpdate(diff);
                    if (TimeElapsed >= Duration)
                    {
                        OnRemoveBuff(BuffRemovalSource.Timeout);
                    }
                }
            }
        }

        public virtual void OnAddBuff() 
        {
            ActivateBuff();

            _game.PacketNotifier.NotifyNPC_BuffAdd2(this, Duration, TimeElapsed);
        }
        public virtual void OnNewBuff() 
        {
        }

        public virtual void OnRemoveBuff(BuffRemovalSource removalSource = BuffRemovalSource.Timeout)
        {
            DeactivateBuff();

            if (!IsHidden)
            {
                _game.PacketNotifier.NotifyNPC_BuffRemove2(this);
            }
        }
    }
}
