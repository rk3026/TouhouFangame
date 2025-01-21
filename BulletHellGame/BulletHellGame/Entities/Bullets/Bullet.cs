using BulletHellGame.Components;

namespace BulletHellGame.Entities.Bullets
{
    public class Bullet : Entity
    {
        public BulletType BulletType { get; private set; }
        public Bullet(BulletType type, Texture2D texture, Vector2 position, List<Rectangle> frameRects = null, double frameDuration = 0.1, bool isAnimating = false) : base(texture, position, frameRects, frameDuration, isAnimating)
        {
            this.BulletType = type;
            // Add components for damage and rotation
            AddComponent(new RotationComponent(this));
            AddComponent(new DamageComponent(25));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // Get the rotation from the RotationComponent, default to 0 if not available
            this.Rotation = GetComponent<RotationComponent>()?.Rotation ?? 0f;
        }
    }
}
