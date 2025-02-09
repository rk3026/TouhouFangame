namespace BulletHellGame.Data.DataTransferObjects
{
    public class PlayerShootingData
    {
        public WeaponData FrontWeapon { get; set; }
        public Dictionary<Vector2, WeaponData> SideWeapons { get; set; } = new();
        public PlayerShootingData() { }
    }
}
