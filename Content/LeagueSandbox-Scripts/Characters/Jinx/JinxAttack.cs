using System;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using LeagueSandbox.GameServer.API;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Enums;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects.Spell.Missile;
using GameServerCore.Domain.GameObjects.Spell.Sector;

namespace Spells
{
    public class JinxBasicAttack : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        IObjAiBase _owner;
        ISpell _spell;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            _spell = spell;
            //ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            //ApiEventManager.OnSpellHit.AddListener(this, spell, OnSpellHit, false);
            ApiEventManager.OnHitUnit.AddListener(this, owner, OnHitUnit, false);
        }

        private void OnHitUnit(IDamageData damageData)
        {
            AddBuff("JinxQRamp", 2.5f, 1, _spell, _owner, _owner);
        }

        private void OnSpellHit(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            //AddBuff("JinxQRamp", 2.5f, 1, _spell, _owner, _owner);
        }

        public void OnLaunchAttack(ISpell spell)
        {
            //_owner.SetAutoAttackSpell("JinxBasicAttack2", false);
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack2", false);
            //SpellCast(spell.CastInfo.Owner, 3, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
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

    public class JinxBasicAttack2 : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            MissileParameters = new MissileParameters
            {
                Type = MissileType.Target
            }
        };

        IObjAiBase _owner;
        ISpell _spell;
        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
            _owner = owner;
            _spell = spell;
            ApiEventManager.OnLaunchAttack.AddListener(this, owner, OnLaunchAttack, false);
            ApiEventManager.OnSpellHit.AddListener(this, spell, OnSpellHit, false);
        }

        private void OnSpellHit(ISpell spell, IAttackableUnit target, ISpellMissile missile, ISpellSector sector)
        {
            AddBuff("JinxQRamp", 2.5f, 1, _spell, _owner, _owner);
        }

        public void OnLaunchAttack(ISpell spell)
        {
            //_owner.SetAutoAttackSpell("JinxBasicAttack", false);
            //spell.CastInfo.Owner.SetAutoAttackSpell("NasusBasicAttack2", false);
            //SpellCast(spell.CastInfo.Owner, 3, SpellSlotType.ExtraSlots, false, spell.CastInfo.Owner.TargetUnit, Vector2.Zero);
        }


        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
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

    public class JinxCritAttack : ISpellScript
    {
        public ISpellScriptMetadata ScriptMetadata { get; private set; } = new SpellScriptMetadata()
        {
            // TODO
        };

        public void OnActivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell)
        {
        }

        public void OnSpellPreCast(IObjAiBase owner, ISpell spell, IAttackableUnit target, Vector2 start, Vector2 end)
        {
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
