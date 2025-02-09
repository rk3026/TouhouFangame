using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.Components
{
    public class ShootingComponent : IComponent
    {
        public BulletData BulletData { get; set; }
        public float FireRate { get; set; } = 0.1f; // Seconds between shots
        public float TimeSinceLastShot { get; set; }

        public List<Vector2> FireDirections = new List<Vector2>();

        public ShootingComponent(BulletData bulletData) {
            this.BulletData = bulletData;
            TimeSinceLastShot = FireRate;
        }

        public bool CanShoot()
        {
            return TimeSinceLastShot >= FireRate;
        }
    }
}