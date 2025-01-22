using BulletHellGame.Entities.Bullets;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using BulletHellGame.Entities.Characters.Enemies;

namespace BulletHellGame.Components
{
    public class HitboxComponent : IComponent
    {
        public Rectangle Hitbox { get; private set; }
        private Entity _owner;

        public HitboxComponent(Entity owner, Rectangle hitbox)
        {
            _owner = owner;
            Hitbox = hitbox;
        }

        public void Update(GameTime gameTime)
        {
            // Keep the hitbox position synced with the owner's position
            Hitbox = new Rectangle(
                (int)_owner.Position.X,
                (int)_owner.Position.Y,
                _owner.GetComponent<SpriteComponent>().CurrentFrame.Width,
                _owner.GetComponent<SpriteComponent>().CurrentFrame.Height
            );

            // Check for collisions with other entities
            CheckCollisions();
        }

        private void CheckCollisions()
        {
            foreach (var entity in EntityManager.Instance.GetActiveEntities()) // Ensure a stable collection during iteration
            {
                if (entity == _owner) continue; // Skip self-collision

                var otherHitbox = entity.GetComponent<HitboxComponent>();
                if (otherHitbox != null && Hitbox.Intersects(otherHitbox.Hitbox))
                {
                    // Handle collision logic
                    OnCollision(entity);
                }
            }
        }

        private void OnCollision(Entity other)
        {
            if (_owner is Enemy && other is Bullet bullet)
            {
                var health = _owner.GetComponent<HealthComponent>();
                if (health != null)
                {
                    health.TakeDamage(bullet.GetComponent<DamageComponent>().CalculateDamage());
                    EntityManager.Instance.QueueEntityForRemoval(bullet); // Queue bullet for removal
                }

                // Trigger the red flash effect
                var spriteEffect = _owner.GetComponent<SpriteEffectComponent>();
                spriteEffect?.FlashRed();
            }
        }

    }
}
