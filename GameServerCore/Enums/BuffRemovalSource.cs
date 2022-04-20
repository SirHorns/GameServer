namespace GameServerCore.Enums
{
    public enum BuffRemovalSource
    {
        NOT_YET_REMOVED,
        MANUAL,
        TIMEOUT,
        STACK_FALL_OFF,
        DEATH,
        CLEANSED,
        REPLACED,
        SHIELD_BREAK,
        SPELL_SHIELD_POP,
        REJECTED,  // placeholder for rejected Debuffs, even though they will never actually be removed since they are never added in the first place
    }
}
