using GameServerCore.Domain;
using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;

namespace GameServerLib.GameObjects.AttackableUnits
{
    public class ShieldData : IShieldData
    {
        /// <summary>
        /// Whether or not the shield being modified is of the Physical type.
        /// </summary>
        public bool Physical { get; set; }
        /// <summary>
        /// Whether or not the shield being modified is of the Magical type.
        /// </summary>
        public bool Magical { get; set; }
        /// <summary>
        /// Whether the shield should stay static or fade.
        /// </summary>
        public bool StopShieldFade { get; set; }
        /// <summary>
        /// Shield amount.
        /// </summary>
        public float Amount { get; set; }
    }
}
