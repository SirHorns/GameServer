using GameServerCore.Enums;

namespace GameServerCore.Domain.GameObjects
{
    public interface IShield
    {
        float shieldHealthCurrent { get; }
        float shieldHealthBase { get;}
        float shieldHealthMax { get;}
        float ShieldHealthBonus { get; }

        IBuff sourceBuff { get; }
        ShieldType shieldType { get; }
        bool isTrueShield { get; }

        bool initializing { get; }

        void RemoveShield(bool stopShieldFade);

        void SetShieldHealthCurrent(float newValue, IDamageData damageInstance);

        void SetShieldHealthMax(float newValue, bool applyToCurrentHealth);

        void SetShieldHealthBonus(float newValue, bool applyToCurrentHealth);
    }
}
