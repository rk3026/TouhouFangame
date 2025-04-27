using Assimp;

namespace BulletHellGame.Presentation.UI
{
    public class ParallaxLayer
    {
        private Texture2D _texture;
        private Rectangle _sourceRect;
        private Rectangle _parallaxArea;
        private float _speed;
        private float _scrollOffset;

        public ParallaxLayer(Texture2D texture, Rectangle sourceRect, Rectangle parallaxArea, float speed)
        {
            _texture = texture;
            _sourceRect = sourceRect;
            _parallaxArea = parallaxArea;
            _speed = speed;
            _scrollOffset = 0f;
        }

        public void Update(GameTime gameTime, SpriteEffects se = SpriteEffects.None)
        {
            float direction = 1f;
            if ((se & SpriteEffects.FlipVertically) != 0)
                direction = -1f;

            _scrollOffset += direction * _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Wrap the scroll offset for both directions
            if (_scrollOffset >= _sourceRect.Height)
                _scrollOffset -= _sourceRect.Height;
            else if (_scrollOffset <= -_sourceRect.Height)
                _scrollOffset += _sourceRect.Height;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects screenFlip = SpriteEffects.None)
        {
            // Handle negative offset correctly
            int topClippedAmount = (int)(_scrollOffset % _sourceRect.Height);
            if (topClippedAmount < 0)
                topClippedAmount += _sourceRect.Height;

            float startY = _parallaxArea.Top - topClippedAmount;
            bool flipVertically = (screenFlip & SpriteEffects.FlipVertically) != 0;

            for (int i = 0; i <= _parallaxArea.Height / _sourceRect.Height + 1; i++)
            {
                float tileY = startY + i * _sourceRect.Height;

                // Calculate the visible part of the tile that fits inside the parallax area
                int visibleTop = (int)Math.Max(_parallaxArea.Top, tileY);
                int visibleBottom = (int)Math.Min(_parallaxArea.Bottom, tileY + _sourceRect.Height);
                int visibleHeight = visibleBottom - visibleTop;

                if (visibleHeight > 0)
                {
                    int sourceY = _sourceRect.Y + (visibleTop - (int)tileY);
                    Rectangle clippedSourceRect = new Rectangle(_sourceRect.X, sourceY, _sourceRect.Width, visibleHeight);

                    Rectangle destinationRect;

                    if (flipVertically)
                    {
                        // Flip vertically: draw from the bottom up
                        int flippedY = _parallaxArea.Bottom - (visibleTop - _parallaxArea.Top) - visibleHeight;
                        destinationRect = new Rectangle(_parallaxArea.Left, flippedY, _parallaxArea.Width, visibleHeight);
                    }
                    else
                    {
                        destinationRect = new Rectangle(_parallaxArea.Left, visibleTop, _parallaxArea.Width, visibleHeight);
                    }

                    spriteBatch.Draw(_texture, destinationRect, clippedSourceRect, Color.White, 0f, Vector2.Zero, screenFlip, 0f);
                }
            }
        }

    }
}
