using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using System.Linq;

public class SpriteComponent : IComponent
{
    public SpriteData SpriteData { get; private set; }
    public Color Color { get; set; } = Color.White;
    public Vector2 Position { get; set; } = Vector2.Zero;
    public float Rotation { get; set; } = 0f;
    public Vector2 Scale { get; set; } = Vector2.One;
    public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

    private string _currentAnimation;
    private int _frameIndex;
    private double _timeSinceLastFrame;
    private double _frameDuration = 0.15; // Default frame duration
    private bool _isAnimating = false;  // Default animation state
    private bool _loopAnimation = true; // Flag for controlling animation looping
    private bool _isReversing = false; // Flag to indicate when to play the animation in reverse
    private Rectangle _currentRect;

    public Rectangle CurrentFrame => _currentRect;

    public SpriteComponent(SpriteData spriteInfo)
    {
        SpriteData = spriteInfo ?? throw new ArgumentNullException(nameof(spriteInfo));

        // Initialize animation and set the first rectangle
        _currentAnimation = SpriteData.Animations.Keys.FirstOrDefault()
            ?? throw new InvalidOperationException("No animations found in the provided SpriteData.");

        _frameIndex = 0;
        _timeSinceLastFrame = 0;
        _currentRect = GetCurrentFrameRect();
    }

    public void Update(GameTime gameTime)
    {
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
                    _frameIndex--; // Play animation in reverse
                    if (_frameIndex < 0)
                    {
                        _frameIndex = 0; // Ensure frame index doesn't go negative
                        _isReversing = false; // Stop reversing once it reaches the first frame
                    }
                }
                else
                {
                    _frameIndex++; // Regular forward play
                    if (_frameIndex >= SpriteData.Animations[_currentAnimation].Count)
                    {
                        if (_loopAnimation)
                        {
                            _frameIndex = 0; // Loop the animation if flag is set
                        }
                        else
                        {
                            _frameIndex = SpriteData.Animations[_currentAnimation].Count - 1; // Don't loop, stop at last frame
                            _isAnimating = false; // Stop animating after the last frame
                        }
                    }
                }

                _timeSinceLastFrame = 0;
                _currentRect = GetCurrentFrameRect();
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            SpriteData.Texture,
            Position,
            CurrentFrame,
            Color,
            Rotation,
            SpriteData.Origin,
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

    public void SwitchAnimation(string animationName, bool loop = true)
    {
        if (_currentAnimation == animationName) return;

        if (!SpriteData.HasAnimation(animationName))
        {
            throw new ArgumentException($"Animation '{animationName}' not found in the SpriteData.");
        }

        _currentAnimation = animationName;
        _loopAnimation = loop;  // Set looping behavior
        _frameIndex = 0;
        _timeSinceLastFrame = 0;
        _currentRect = GetCurrentFrameRect();
    }

    private Rectangle GetCurrentFrameRect()
    {
        return SpriteData.Animations.TryGetValue(_currentAnimation, out var frames) && frames.Count > 0
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
        if (frameIndex >= 0 && frameIndex < SpriteData.Animations[_currentAnimation].Count)
        {
            _frameIndex = frameIndex;
            _currentRect = GetCurrentFrameRect();
        }
    }

    public void SetReversing(bool flag)
    {
        _isReversing = flag;
        _frameIndex = SpriteData.Animations[_currentAnimation].Count - 1; // Start at the last frame for reverse
    }
}

