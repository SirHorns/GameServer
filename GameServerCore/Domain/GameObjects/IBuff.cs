using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Enums;
using GameServerCore.Scripting.CSharp;
using System.Collections.Generic;

namespace GameServerCore.Domain.GameObjects
{
    public interface IBuff : IStackable, IUpdate
    {
        /// <summary>
        /// Type of buff to add. Determines how this buff interacts with mechanics of the game. Refer to BuffType.
        /// </summary>
        BuffType BuffType { get; }
        /// <summary>
        /// How this buff should be added and treated when adding new buffs of the same name.
        /// </summary>
        BuffAddType BuffAddType { get; }
        /// <summary>
        /// What caused the Removal of this Buff.
        /// </summary>
        BuffRemovalSource BuffRemovalSource { get; }
        /// <summary>
        /// Internal name of this buff.
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Total time this buff should be applied to its target.
        /// </summary>
        float Duration { get; }
        /// <summary>
        /// Time since this buff's timer started.
        /// </summary>
        float TimeElapsed { get; }
        /// <summary>
        /// Time remaining on this buff before it expires.
        /// </summary>
        float TimeRemaining { get; }
        /// <summary>
        /// Slot of this buff instance. Maximum allowed is 255 due to packets.
        /// </summary>
        byte Slot { get; }
        /// <summary>
        /// Whether or not this buff should be shown on clients' buff bar (HUD).
        /// </summary>
        bool IsHidden { get; }
        /// <summary>
        /// Whether this buff is the root buff in a stack of buffs.
        /// </summary>
        bool IsRootBuff { get; }
        /// <summary>
        /// The root buff this buff belongs to as a part of a stack. (Uses to refrence back to if this buff is accidently called intead of the root buff.)
        /// </summary>
        IBuff RootBuff { get; set; }
        /// <summary>
        /// Spell which caused this buff to be applied.
        /// </summary>
        ISpell OriginSpell { get; }
        /// <summary>
        /// Unit which applied this buff to its target.
        /// </summary>
        IObjAiBase SourceUnit { get; }
        /// <summary>
        /// Unit which has this buff applied to it.
        /// </summary>
        IAttackableUnit TargetUnit { get; }
        /// <summary>
        /// Script instance for this buff. Casting to a specific buff class gives access its functions and variables.
        /// </summary>
        IBuffGameScript BuffScript { get; }
        /// <summary>
        /// All status effects applied by this buff.
        /// </summary>
        Dictionary<StatusFlags, bool> StatusEffects { get; }
        /// <summary>
        /// Used to update player buff tool tip values.
        /// </summary>
        IToolTipData ToolTipData { get; }
        /// <summary>
        /// List of buffs related to a stack.(Excluding itself);
        /// </summary>
        List<IBuff> StackList { get; }

        /// <summary>
        /// Used to load the script for the buff.
        /// </summary>
        void LoadScript();
        void ActivateBuff();
        void DeactivateBuff();
        bool Elapsed();
        IStatsModifier GetStatsModifier();
        void SetStatusEffect(StatusFlags flag, bool enabled);
        bool IsBuffInfinite();
        bool IsBuffSame(string buffName);
        void ResetTimeElapsed();
        void SetSlot(byte slot);
        void SetToolTipVar<T>(int tipIndex, T value) where T : struct;
        void SetIsRootBuff(bool root);
        public void SetStackList(List<IBuff> buffStackLsit);
        public void AddBuffStackList(IBuff b);
        public void RemoveBuffStackList(IBuff b);
        public void RemoveBuffStackList(int slot = 0);
        virtual void OnAddBuff() { }
        virtual void OnNewBuff(IBuff b) { }
        virtual void OnRemoveBuff() { }
    }
}
