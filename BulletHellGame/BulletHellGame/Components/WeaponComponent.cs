using BulletHellGame.Entities;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Factories;

namespace BulletHellGame.Components
{
    public class WeaponComponent : IComponent
    {
        private Entity _owner;
        private float _fireRate = 0.2f; // Seconds between shots
        private float _timeSinceLastShot = 0f;

        public WeaponComponent(Entity owner)
        {
            _owner = owner;
        }

        public void Update(GameTime gameTime)
        {
            _timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Shoot()
        {
            if (_timeSinceLastShot >= _fireRate)
            {
                // Define the offset for bullet spawn relative to the owner's position
                Vector2 bulletPosition = _owner.Position + new Vector2(16, -10);

                // Define velocities for each bullet
                Vector2 straightUpVelocity = new Vector2(0, -500);      // Straight up
                Vector2 leftUpVelocity = new Vector2(-300, -400);       // Diagonally left (northwest)
                Vector2 rightUpVelocity = new Vector2(300, -400);       // Diagonally right (northeast)

                // Create three bullets with their respective velocities
                EntityManager.Instance.CreateBullet(BulletType.Standard, bulletPosition, straightUpVelocity);
                EntityManager.Instance.CreateBullet(BulletType.Pellet, bulletPosition, leftUpVelocity);
                EntityManager.Instance.CreateBullet(BulletType.Pellet, bulletPosition, rightUpVelocity);

                // Reset the shot timer
                _timeSinceLastShot = 0f;
            }
        }

    }
}
