namespace BulletHellGame.Entities
{
    public class Sprite
    {
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; } = 0f;
        public Vector2 Scale { get; set; } = Vector2.One;
        public Rectangle? SourceRect { get; set; } // Optional rectangle for drawing a subregion of the texture

        // Animation variables
        private int _frameIndex;
        private double _timeSinceLastFrame;
        private double _frameDuration; // Time per frame in seconds
        internal List<Rectangle> _frameRects; // The list of rectangles representing different frames of the animation
        private bool _isAnimating;

        public Sprite(Texture2D texture, Vector2 position, List<Rectangle> frameRects = null, double frameDuration = 0.1, bool isAnimating = false)
        {
            Color = Color.White;
            Texture = texture;
            Position = position;

            // If frameRects is null, initialize it with a single frame (the whole texture)
            if (frameRects == null || frameRects.Count == 0)
            {
                SourceRect = new Rectangle(0, 0, texture.Width, texture.Height);
                _frameRects = new List<Rectangle> { SourceRect.Value };
            }
            else
            {
                SourceRect = frameRects[0];
                _frameRects = frameRects;
            }

            _frameDuration = frameDuration;
            _isAnimating = isAnimating;
            _frameIndex = 0;
            _timeSinceLastFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (_isAnimating && _frameRects.Count > 1)
            {
                _timeSinceLastFrame += gameTime.ElapsedGameTime.TotalSeconds;

                if (_timeSinceLastFrame >= _frameDuration)
                {
                    _frameIndex = (_frameIndex + 1) % _frameRects.Count; // Cycle through frames
                    _timeSinceLastFrame = 0;
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // For non-animated sprites, it will draw the static SourceRect or the first frame.
            Rectangle sourceRect = _isAnimating && _frameRects.Count > 1
                ? _frameRects[_frameIndex]
                : _frameRects[0]; // Use first frame when not animated

            spriteBatch.Draw(
                Texture,
                Position,
                sourceRect, // Draw the current frame or static rectangle
                Color,
                Rotation,  // Apply rotation
                Vector2.Zero, // Origin at the top-left corner (will be adjusted by rotation)
                Scale,
                SpriteEffects.None,
                0f
            );
        }

        // Optional method to start or stop animation
        public void SetAnimationState(bool isAnimating)
        {
            _isAnimating = isAnimating;
            _frameIndex = 0; // Reset to the first frame when animation state changes
        }
    }
}
