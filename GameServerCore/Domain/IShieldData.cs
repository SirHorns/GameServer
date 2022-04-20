using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerCore.Domain
{
    public interface IShieldData
    {
        /// <summary>
        /// Whether or not the shield being modified is of the Physical type.
        /// </summary>
        bool Physical { get;}
        /// <summary>
        /// Whether or not the shield being modified is of the Magical type.
        /// </summary>
        bool Magical { get;}
        /// <summary>
        /// Whether the shield should stay static or fade.
        /// </summary>
        bool StopShieldFade { get;}
        /// <summary>
        /// Shield amount.
        /// </summary>
        float Amount { get;}
    }
}
