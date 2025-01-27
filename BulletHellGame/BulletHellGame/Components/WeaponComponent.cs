using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.Components
{
    public class WeaponComponent : IComponent
    {
        public BulletData bulletData;
        public float FireRate = 0.1f; // Seconds between shots
        public float TimeSinceLastShot;

        public List<Vector2> FireDirections = new List<Vector2>();

        public WeaponComponent(BulletData bulletData) {
            this.bulletData = bulletData;
            TimeSinceLastShot = FireRate;
        }

        public bool CanShoot()
        {
            return TimeSinceLastShot >= FireRate;
        }
    }
}