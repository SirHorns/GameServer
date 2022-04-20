using LeagueSandbox.GameServer.API;
using GameServerCore.Domain.GameObjects;
using LeagueSandbox.GameServer.Scripting.CSharp;
using System.Numerics;
using GameServerCore.Domain.GameObjects.Spell;
using GameServerCore.Scripting.CSharp;
using GameServerCore.Domain;
using static LeagueSandbox.GameServer.API.ApiFunctionManager;
using System;
using GameServerCore.Enums;

//*=========================================
/*
 * ValkyrieHorns
 * Lastupdated:
 * 
 * Notes:
 * 
 * TODOS:
 * 
 * Known Issues:
*/
//*=========================================

namespace CharScripts
{
    public class CharScriptJanna : ICharScript
    {
        IObjAiBase _owner;
        ISpell _spell;
        public void OnActivate(IObjAiBase owner, ISpell spell = null)
        {
            _owner = owner;
            _spell = spell;
        }

        public void OnDeactivate(IObjAiBase owner, ISpell spell = null)
        {
        }

        public void OnUpdate(float diff)
        {
        }
    }
}

