using BulletHellGame.Data;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Components
{
    public class WeaponComponent : IComponent
    {
        private Entity _owner;
        private SpriteData _spriteData; // Sprite data for the weapon
        private Vector2 _spriteOffset; // Offset for the sprite position relative to the owner
        private string _currentAnimation = "Default"; // Current animation name
        private int _currentFrame = 0; // Current frame in the animation
        private float _frameTime = 0.1f; // Time per frame in seconds
        private float _timeSinceLastFrame = 0f; // Time accumulator for animation
        private float _fireRate = 0.2f; // Seconds between shots
        private float _timeSinceLastShot = 0f;
        private float _rotation = 0f; // Rotation in radians

        public WeaponComponent(Entity owner, SpriteData spriteData, Vector2 spriteOffset)
        {
            _owner = owner;
            _spriteData = spriteData ?? throw new ArgumentNullException(nameof(spriteData));
            _spriteOffset = spriteOffset;

            // Ensure the default animation exists
            if (!_spriteData.HasAnimation(_currentAnimation))
            {
                throw new ArgumentException($"The animation '{_currentAnimation}' is not defined in the provided SpriteData.");
            }
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update shot cooldown
            _timeSinceLastShot += deltaTime;

            // Update animation frame timer
            _timeSinceLastFrame += deltaTime;

            if (_timeSinceLastFrame >= _frameTime)
            {
                _timeSinceLastFrame = 0f;

                // Advance to the next frame in the current animation
                List<Rectangle> frames = _spriteData.GetAnimationFrames(_currentAnimation);
                _currentFrame = (_currentFrame + 1) % frames.Count;
            }

            _rotation += 0.1f; // Adjust rotation speed as needed
        }

        public void Shoot(List<Vector2> firingDirections)
        {
            if (_timeSinceLastShot >= _fireRate)
            {
                Vector2 weaponPosition = _owner.Position + _spriteOffset;

                foreach (var direction in firingDirections)
                {
                    // Spawn a bullet for each firing direction
                    Vector2 bulletVelocity = direction * 500; // Adjust the speed as needed
                    EntityManager.Instance.CreateBullet(BulletType.Standard, weaponPosition, bulletVelocity);
                }

                // Reset the shot timer
                _timeSinceLastShot = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_spriteData != null)
            {
                // Get the current frame of the animation
                List<Rectangle> frames = _spriteData.GetAnimationFrames(_currentAnimation);
                Rectangle sourceRectangle = frames[_currentFrame];

                // Draw the sprite using the weapon's offset
                Vector2 drawPosition = _owner.Position + _spriteOffset;

                Vector2 origin = _spriteData.Origin + new Vector2(sourceRectangle.Width /2, sourceRectangle.Y / 2);

                spriteBatch.Draw(
                    _spriteData.Texture,    // Texture
                    new Vector2(drawPosition.X+sourceRectangle.Width/2, drawPosition.Y+sourceRectangle.Height/2),           // Position
                    sourceRectangle,        // Source rectangle
                    Color.White,            // Tint
                    _rotation,              // Rotation (added rotation)
                    origin,     // Origin
                    1f,                     // Scale
                    SpriteEffects.None,     // Effects
                    0f                      // Layer depth
                );
            }
        }

        // Optionally, you can expose a method to manually set the rotation if needed
        public void SetRotation(float rotation)
        {
            _rotation = rotation;
        }
    }
}
