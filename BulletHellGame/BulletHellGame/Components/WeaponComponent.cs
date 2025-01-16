using BulletHellGame.Entities;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Factories;
using BulletHellGame.Managers;

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

            // Fire a bullet if Space is pressed and fire rate allows it
            if (InputManager.KeyDown(Keys.Space) && _timeSinceLastShot >= _fireRate)
            {
                Shoot();
                _timeSinceLastShot = 0f;
            }
        }

        private void Shoot()
        {
            // Create a bullet at the player's position
            Vector2 bulletPosition = _owner.Position + new Vector2(16, -10); // Offset for bullet spawn
            Vector2 bulletSpeed = new Vector2(0, -500); // Upward Velocity
            EntityManager.Instance.CreateBullet(BulletType.Standard, bulletPosition, bulletSpeed);
        }
    }
}
