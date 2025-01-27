using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;
using System.Linq;

public class SpriteComponent : IComponent
{
    public SpriteData SpriteData { get; set; }
    public Color Color { get; set; } = Color.White;
    public float Rotation { get; set; } = 0f;
    public Vector2 Scale { get; set; } = Vector2.One;
    public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

    private string _currentAnimation;
    private int _frameIndex;
    private double _timeSinceLastFrame;
    private double _frameDuration = 0.15; // Default frame duration
    private bool _isAnimating = false;
    private bool _loopAnimation = true;
    private bool _isReversing = false;
    private Rectangle _currentRect;

    // Flash properties
    private bool _isFlashing = false;
    private float _flashDuration = 0f;

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

    public void Update(GameTime gameTime)
    {
        // Handle flashing logic
        if (_isFlashing)
        {
            _flashDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_flashDuration <= 0)
            {
                Color = Color.White;
                _isFlashing = false;
            }
        }

        if (_currentAnimation == null || !SpriteData.Animations.ContainsKey(_currentAnimation))
        {
            // No animation to update
            return;
        }

        if (SpriteData.Animations[_currentAnimation].Count > 1)
        {
            _isAnimating = true;
        }

        if (_isAnimating)
        {
            _timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

            if (_timeSinceLastFrame >= _frameDuration)
            {
                if (_isReversing)
                {
                    _frameIndex--;
                    if (_frameIndex < 0)
                    {
                        _frameIndex = 0;
                        _isReversing = false;
                    }
                }
                else
                {
                    _frameIndex++;
                    if (_frameIndex >= SpriteData.Animations[_currentAnimation].Count)
                    {
                        if (_loopAnimation)
                        {
                            _frameIndex = 0;
                        }
                        else
                        {
                            _frameIndex = SpriteData.Animations[_currentAnimation].Count - 1;
                            _isAnimating = false;
                        }
                    }
                }

                _timeSinceLastFrame = 0;
                _currentRect = GetCurrentFrameRect();
            }
        }
    }

    public void FlashRed(float duration = 0.2f)
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
            throw new ArgumentException($"Animation '{animationName}' not found in the SpriteData.");
        }

        _currentAnimation = animationName;
        _loopAnimation = loop;
        _frameIndex = 0;
        _timeSinceLastFrame = 0;
        _currentRect = GetCurrentFrameRect();
    }

    private Rectangle GetCurrentFrameRect()
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
