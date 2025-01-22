using BulletHellGame.Data;
using System.Linq;

namespace BulletHellGame.Components
{
    public class SpriteComponent : IComponent
    {
        public SpriteData SpriteInfo { get; private set; }
        public Color Color { get; set; } = Color.White;
        public Vector2 Position { get; set; }
        public float Rotation { get; set; } = 0f;
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

        private string _currentAnimation;
        private int _frameIndex;
        private double _timeSinceLastFrame;
        private double _frameDuration = 0.2; // Default frame duration
        private bool _isAnimating = false;  // Default animation state
        private Rectangle _currentRect;

        public Rectangle CurrentFrame => _currentRect;

        public SpriteComponent(SpriteData spriteInfo, Vector2 position)
        {
            SpriteInfo = spriteInfo ?? throw new ArgumentNullException(nameof(spriteInfo));
            Position = position;

            // Initialize animation and set the first rectangle
            _currentAnimation = SpriteInfo.Animations.Keys.FirstOrDefault()
                ?? throw new InvalidOperationException("No animations found in the provided SpriteData.");

            _frameIndex = 0;
            _timeSinceLastFrame = 0;
            _currentRect = GetCurrentFrameRect();
        }

        public void Update(GameTime gameTime)
        {
            if (SpriteInfo.Animations[_currentAnimation].Count > 1)
            {
                _isAnimating = true;
            }
            if (_isAnimating)
            {
                _timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

                if (_timeSinceLastFrame >= _frameDuration)
                {
                    _frameIndex = (_frameIndex + 1) % SpriteInfo.Animations[_currentAnimation].Count;
                    _timeSinceLastFrame = 0;
                    _currentRect = GetCurrentFrameRect();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                SpriteInfo.Texture,
                Position,
                CurrentFrame,
                Color,
                Rotation,
                SpriteInfo.Origin,
                Scale,
                SpriteEffect,
                0f
            );
        }

        public void SetAnimationState(bool isAnimating)
        {
            _isAnimating = isAnimating;

            if (!isAnimating)
            {
                _frameIndex = 0;
                _timeSinceLastFrame = 0;
                _currentRect = GetCurrentFrameRect();
            }
        }

        public void SwitchAnimation(string animationName)
        {
            if (_currentAnimation == animationName) return;

            if (!SpriteInfo.HasAnimation(animationName))
            {
                throw new ArgumentException($"Animation '{animationName}' not found in the SpriteData.");
            }

            _currentAnimation = animationName;
            _frameIndex = 0;
            _timeSinceLastFrame = 0;
            _currentRect = GetCurrentFrameRect();
        }

        private Rectangle GetCurrentFrameRect()
        {
            return SpriteInfo.Animations.TryGetValue(_currentAnimation, out var frames) && frames.Count > 0
                ? frames[_frameIndex]
                : Rectangle.Empty;
        }

        public void Reset()
        {
            _frameIndex = 0;
            _timeSinceLastFrame = 0;
            _currentRect = GetCurrentFrameRect();
        }

        public void SetFrameIndex(int frameIndex)
        {
            // Check if the index is valid before setting it
            if (frameIndex >= 0 && frameIndex < SpriteInfo.Animations[_currentAnimation].Count)
            {
                _frameIndex = frameIndex;
                _currentRect = GetCurrentFrameRect(); // Update the frame rectangle
            }
        }

    }
}
