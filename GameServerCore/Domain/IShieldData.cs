using GameServerCore.Domain.GameObjects;
using GameServerCore.Enums;

namespace GameServerCore.Domain
{
    public interface IShieldData
    {
        /// <summary>
        /// Whether or not the shield being modified is of the Physical type.
        /// </summary>
        bool Physical { get; }
        /// <summary>
        /// Whether or not the shield being modified is of the Magical type.
        /// </summary>
        bool Magical { get; }
        /// <summary>
        /// Whether the shield should stay static or fade.
        /// </summary>
        bool StopShieldFade { get; }
        /// <summary>
        /// Shield Amount to apply to the unit.
        /// </summary>
        float Amount { get; }
    }
}