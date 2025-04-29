using BulletHellGame.Logic.Components;

namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class WeaponData
    {
        public BulletData BulletData { get; set; }
        public float FireRate { get; set; }
        public float TimeSinceLastShot { get; set; }

        public List<Vector2> FireDirections { get; set; } = new();

        public string MovementPattern { get; set; } = string.Empty;
    }
}
