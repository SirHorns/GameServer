using System;
using System.Collections.Generic;
using System.Text;

namespace GameServerCore.Enums
{
    public enum BuffRemovalSource
    {
        NotYetRemoved,
        Manual,
        Timeout,
        StackFalloff,
        Death,
        Cleansed,
        Replaced,
        ShieldBreak,
        ShieldPop,
        Rejected
    }
}
