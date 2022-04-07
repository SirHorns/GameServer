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
 * Lastupdated: 4/5/2022
 * 
 * Notes:
 * Why did I make this?
 * 
 * TODOS:
 * 
 * Known Issues:
*/
//*=========================================

namespace CharScripts
{
    public class CharScriptNunuBot : ICharScript
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

        float lastTick = 0.0f;
        float laughTrigger = 0.0f;

        Random rng = new Random();
        public void OnUpdate(float diff)
        {
            lastTick += diff;
            Console.WriteLine(lastTick / 1000 + " : " + laughTrigger);
            if (lastTick / 1000 >= laughTrigger)
            {
                lastTick = 0.0f;
                laughTrigger = rng.Next(1, 12);
            }
        }
    }
}

