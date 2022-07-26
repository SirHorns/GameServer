namespace GameServerCore.Enums
{
    public enum BuffRemovalSource
    {
        NOT_YET_REMOVED,
        MANUAL,
        TIMEOUT,
        STACK_FALLOFF,
        DEATH,
        CLEANSED,
        REPLACED,
        SHIELD_BREAK,
        SPELL_SHIELD_POP,
        REJECTED,  // placeholder for rejected Buffs, even though they will never actually be removed since they are never added in the first place
    }
}