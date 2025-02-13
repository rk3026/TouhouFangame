namespace BulletHellGame.Data.DataTransferObjects
{
    public class ShotJsonDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PowerLevelJsonDto> PowerLevels { get; set; }
    }

    public class PowerLevelJsonDto
    {
        public int Level { get; set; }
        public List<WeaponJsonDto> MainWeapons { get; set; }
        public List<SideWeaponJsonDto> SideWeapons { get; set; }
    }

    public class WeaponJsonDto
    {
        public string SpriteName { get; set; }
        public float FireRate { get; set; }
        public List<Vector2Dto> FireDirections { get; set; }
        public BulletJsonDto BulletData { get; set; }
    }

    public class SideWeaponJsonDto : WeaponJsonDto
    {
        public Vector2Dto Offset { get; set; }
    }

    public class BulletJsonDto
    {
        public string SpriteName { get; set; }
        public int Damage { get; set; }
        public string BulletType { get; set; }
    }

    public class Vector2Dto
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}