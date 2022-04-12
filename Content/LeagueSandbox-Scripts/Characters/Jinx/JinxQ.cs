using System.Numerics;
using GameServerCore.Enums;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using LeagueSandbox.GameServer.Scripting.CSharp;
using GameServerCore.Scripting.CSharp;
using LeagueSandbox.GameServer.API;
using System;
using System.Collections.Generic;

namespace Spells
{
    public class JinxQ : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            TriggersSpellCasts = true,
        };

        IObjAiBase _owner;
        Dictionary<string, string> PowPowAnimPairs;
        Dictionary<string, string> FishbonesAnimPairs;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            PowPowAnimPairs = new Dictionary<string, string>();
            FishbonesAnimPairs = new Dictionary<string, string>();
            SetAnimPairs();
            ApiEventManager.onspell
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        bool Toggle = false;
        IBuff buffPowPow;
        IBuff buffFishbones;
        ISpell _autoAttackSpell;
        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
            //owner.PlayAnimation("spell1", .5f, flags: AnimationFlags.Unknown8 | AnimationFlags.UniqueOverride | AnimationFlags.Lock);



            
            _owner.PlayAnimation("spell1", 1.0f, flags: AnimationFlags.Unknown8);
            if (Toggle)
            {
                owner.RemoveBuff(buffFishbones);
                _owner.PlayAnimation("Minigun_to_RLauncher", 1.0f, flags: AnimationFlags.Unknown8 | AnimationFlags.Lock | AnimationFlags.UniqueOverride);
                spell.SetAutocast(255, 255);
                SetAutocast(_owner.GetSpell("JinxQ"));
                owner.SetAnimStates(new Dictionary<string, string>());

                buffPowPow = AddBuff("JinxQIcon", 0f, 1, spell, owner, owner, true);

                Toggle = false;
            }
            else
            {
                owner.RemoveBuff(buffPowPow);
                _owner.PlayAnimation("Minigun_to_RLauncher", 1.0f, flags: AnimationFlags.Unknown8 | AnimationFlags.Lock | AnimationFlags.UniqueOverride);
                owner.SetAnimStates(FishbonesAnimPairs);
                _autoAttackSpell = _owner.GetSpell("JinxQAttack");
                spell.SetAutocast(47, 46);

                buffFishbones = AddBuff("JinxQ", 0f, 1, spell, owner, owner, true);

                Toggle = true;
            }

            //_owner.PlayAnimation("spell1", 1.0f, flags: AnimationFlags.Unknown8);
            if (Toggle)
            {
                //owner.RemoveBuff(buffFishbones);
                //buffPowPow = AddBuff("JinxQIcon", 0f, 1, spell, owner, owner, true);

                //_owner.PlayAnimation("spell1", 1.0f, flags: AnimationFlags.Unknown8);
                //_owner.PlayAnimation("Spell1", 1.0f, flags: AnimationFlags.Unknown8  | AnimationFlags.UniqueOverride | AnimationFlags.Lock);
                //_owner.SetAnimStates(PowPowAnimPairs);
                //_owner.SetAnimStates(FishbonesAnimPairs);

                //Toggle = false;
            }
            else
            {
                //owner.RemoveBuff(buffPowPow);
                //buffFishbones = AddBuff("JinxQ", 0f, 1, spell, owner, owner, true);

                //_owner.PlayAnimation("R_ATTACK1", 1.0f, flags: AnimationFlags.Unknown8 );
                //_owner.PlayAnimation("RLauncher_to_Minigun", 1.0f, flags: AnimationFlags.Unknown8 | AnimationFlags.UniqueOverride | AnimationFlags.Lock);
                //_owner.SetAnimStates(FishbonesAnimPairs);
                //_owner.SetAnimStates(PowPowAnimPairs);

                //Toggle = true;
            }
        }

        private void SetAnimPairs()
        {
            PowPowAnimPairs.Add("RUN", "RUN");
            PowPowAnimPairs.Add("RUN2", "RUN2");
            PowPowAnimPairs.Add("RUN_FAST", "RUN_FAST");
            PowPowAnimPairs.Add("IDLE1", "IDLE1");
            PowPowAnimPairs.Add("IDLE2", "IDLE2");
            PowPowAnimPairs.Add("IDLE3", "IDLE3");
            PowPowAnimPairs.Add("DEATH", "DEATH");
            PowPowAnimPairs.Add("ATTACK1", "ATTACK1");
            PowPowAnimPairs.Add("ATTACK2", "ATTACK2");
            PowPowAnimPairs.Add("SPELL1", "SPELL1");
            PowPowAnimPairs.Add("SPELL2", "SRPELL2");
            PowPowAnimPairs.Add("SPELL3", "SPELL3");
            PowPowAnimPairs.Add("SPELL3_RUN", "SPELL3_RUN");
            PowPowAnimPairs.Add("SPELL4", "SPELL4");
            PowPowAnimPairs.Add("TAUNT", "TAUNT");
            PowPowAnimPairs.Add("JOKE", "JOKE");
            PowPowAnimPairs.Add("LAUGH", "LAUGH");



            FishbonesAnimPairs.Add("RUN", "R_RUN");
            FishbonesAnimPairs.Add("RUN2", "R_RUN2");
            FishbonesAnimPairs.Add("RUN_FAST", "R_RUN_FAST");
            FishbonesAnimPairs.Add("IDLE1", "R_IDLE1");
            FishbonesAnimPairs.Add("IDLE2", "R_IDLE2");
            FishbonesAnimPairs.Add("IDLE3", "R_IDLE3");
            FishbonesAnimPairs.Add("DEATH", "R_DEATH");
            FishbonesAnimPairs.Add("ATTACK1", "R_ATTACK1");
            FishbonesAnimPairs.Add("ATTACK2", "R_ATTACK2");
            FishbonesAnimPairs.Add("SPELL1", "R_SPELL1");
            FishbonesAnimPairs.Add("SPELL2", "R_SPELL2");
            FishbonesAnimPairs.Add("SPELL3", "R_SPELL3");
            FishbonesAnimPairs.Add("SPELL3_RUN", "R_SPELL3_RUN");
            FishbonesAnimPairs.Add("SPELL4", "R_SPELL4");
            FishbonesAnimPairs.Add("TAUNT", "R_TAUNT");
            FishbonesAnimPairs.Add("JOKE", "R_JOKE");
            FishbonesAnimPairs.Add("LAUGH", "R_LAUGH");
        }

        public void OnSpellCast(ISpell spell)
        {
        }

        public void OnSpellPostCast(ISpell spell)
        {
        }

        public void OnSpellChannel(ISpell spell)
        {
        }

        public void OnSpellChannelCancel(ISpell spell, ChannelingStopSource reason)
        {
        }

        public void OnSpellPostChannel(ISpell spell)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}
