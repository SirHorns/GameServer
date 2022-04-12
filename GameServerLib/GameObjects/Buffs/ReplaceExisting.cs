using GameServerCore.Domain.GameObjects;
using GameServerCore.Domain.GameObjects.Spell;


using LeagueSandbox.GameServer.Logging;
using log4net;

namespace LeagueSandbox.GameServer.GameObjects
{
    public class BuffReplaceExisting : Buff, IBuff
    {
        protected readonly ILog Logger;
        public BuffReplaceExisting(
            Game game,
            string buffName,
            float duration, 
            int stacks, 
            ISpell originSpell,
            IAttackableUnit onto,
            IObjAiBase from,
            bool infiniteDuration = false
            ) : base(game, buffName, duration, stacks, originSpell, onto, from, infiniteDuration)
        {
            Logger = LoggerProvider.GetLogger();
        }


        public override void AddBuff()
        {
            //TODO: This logic could be applied in several ways so for now this will just deactivate and reactivate the buff this is attached to.
            DeactivateBuff();
            ActivateBuff();
        }
        public override void RemoveBuff()
        {
            DeactivateBuff();
        }
    }
}