using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;

namespace GameServerLib.GameObjects.AttackableUnits
{
    class ShieldData : IShieldData
    {
        public bool Physical { get; set; }
        public bool Magical { get; set; }
        public bool StopShieldFade { get; set; }
        public float Amount { get; set; }
    }
}
