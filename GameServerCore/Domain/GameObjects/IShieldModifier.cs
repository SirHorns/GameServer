namespace GameServerCore.Domain.GameObjects
{
    public interface IShieldModifier
    {
        public bool Physical { get; set; }
        public bool Magical { get; set; }
        public bool StopShieldFade { get; set; }
        public float Amount { get; set; }
    }
}
