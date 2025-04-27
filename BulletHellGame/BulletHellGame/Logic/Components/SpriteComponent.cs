using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using System.Linq;

namespace BulletHellGame.Logic.Components
{
    public class SpriteComponent : IComponent
    {
        public SpriteData SpriteData { get; set; }
        public Color Color { get; set; } = Color.White;
        public float CurrentRotation { get; set; } = 0f;
        public float RotationSpeed { get; set; } = 0f;
        public Vector2 Scale { get; set; } = Vector2.One;
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

        public string _currentAnimation;
        public int _frameIndex;
        public double _timeSinceLastFrame;
        public double _frameDuration = 0.15; // Default frame duration
        public bool _isAnimating = false;
        public bool _loopAnimation = true;
        public bool _isReversing = false;
        public Rectangle _currentRect;

        // Flash properties
        public bool _isFlashing = false;
        public float _flashDuration = 0f;

        public Rectangle CurrentFrame => _currentRect;

        public SpriteComponent(SpriteData spriteData = null)
        {
            SpriteData = spriteData ?? TextureManager.Instance.GetDefaultSpriteData();

            if (SpriteData.Animations != null && SpriteData.Animations.Count > 0)
            {
                _currentAnimation = SpriteData.Animations.Keys.First();
                _currentRect = GetCurrentFrameRect();
            }
            else
            {
                _currentAnimation = null; // No animation
                _currentRect = SpriteData.Texture.Bounds; // Default to the full texture
            }
        }

        public void FlashRed(float duration = 0.1f)
        {
            _isFlashing = true;
            _flashDuration = duration;
            Color = Color.Red;
        }

        public void SetAnimationState(bool isAnimating)
        {
            if (_currentAnimation == null) return;

            _isAnimating = isAnimating;

            if (!isAnimating)
            {
                _frameIndex = 0;
                _timeSinceLastFrame = 0;
                _currentRect = GetCurrentFrameRect();
            }
        }

        public void SwitchAnimation(string animationName, bool loop = true)
        {
            if (_currentAnimation == animationName) return;

            if (!SpriteData.HasAnimation(animationName))
            {
                return;
                //throw new ArgumentException($"Animation '{animationName}' not found in the SpriteName.");
            }

            _currentAnimation = animationName;
            _loopAnimation = loop;
            _frameIndex = 0;
            _timeSinceLastFrame = 0;
            _currentRect = GetCurrentFrameRect();
        }

        public Rectangle GetCurrentFrameRect()
        {
            if (_currentAnimation == null || !SpriteData.Animations.TryGetValue(_currentAnimation, out var frames) || frames.Count == 0)
            {
                return SpriteData.Texture.Bounds; // Default to full texture if no frames exist
            }

            return frames[_frameIndex];
        }

        public void Reset()
        {
            if (_currentAnimation == null) return;

            _frameIndex = 0;
            _timeSinceLastFrame = 0;
            _currentRect = GetCurrentFrameRect();
            _isFlashing = false;
            Color = Color.White;
        }

        public void SetFrameIndex(int frameIndex)
        {
            if (_currentAnimation == null) return;

            if (frameIndex >= 0 && frameIndex < SpriteData.Animations[_currentAnimation].Count)
            {
                _frameIndex = frameIndex;
                _currentRect = GetCurrentFrameRect();
            }
        }

        public void SetReversing(bool flag)
        {
            if (_currentAnimation == null) return;

            _isReversing = flag;
            _frameIndex = SpriteData.Animations[_currentAnimation].Count - 1;
        }
    }
}