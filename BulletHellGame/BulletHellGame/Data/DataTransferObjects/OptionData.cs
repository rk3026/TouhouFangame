namespace BulletHellGame.Data.DataTransferObjects
{
    public class OptionData
    {
        public string SpriteName { get; set; }
        public Vector2 Offset { get; set; } = Vector2.Zero;
        public List<WeaponData> Weapons { get; set; }
    }
}
