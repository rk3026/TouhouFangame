using BulletHellGame.Entities.Bullets;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using BulletHellGame.Entities.Characters.Enemies;
using System.Diagnostics;

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

        public Entity GetOwner()
        {
            return _owner;
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

            // Queue check for collisions with other entities
            HitboxManager.Instance.EnqueueInsertion(this);
            HitboxManager.Instance.EnqueueCheck(this);
        }

        public void OnCollision(Entity other)
        {
            if (_owner is Enemy && other is Bullet bullet && bullet.IsActive)
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
