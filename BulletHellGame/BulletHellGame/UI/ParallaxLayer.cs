namespace BulletHellGame.UI
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

        public void Update(GameTime gameTime)
        {
            _scrollOffset += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_scrollOffset >= _sourceRect.Height)
                _scrollOffset -= _sourceRect.Height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Calculate how much of the top tile is visible
            int topClippedAmount = (int)_scrollOffset % _sourceRect.Height;

            // Calculate the vertical starting position to ensure tiles align properly
            int startY = _parallaxArea.Top - topClippedAmount;

            // Iterate through the tiles and clip the top and bottom edges
            for (int i = 0; i <= _parallaxArea.Height / _sourceRect.Height + 1; i++)
            {
                int tileY = startY + i * _sourceRect.Height;

                // Calculate the visible part of the tile that fits inside the parallax area
                int visibleTop = Math.Max(_parallaxArea.Top, tileY);
                int visibleBottom = Math.Min(_parallaxArea.Bottom, tileY + _sourceRect.Height);
                int visibleHeight = visibleBottom - visibleTop;

                if (visibleHeight > 0) // Only draw if part of the tile is visible
                {
                    // Calculate the source rectangle to only draw the visible part of the tile
                    int sourceY = _sourceRect.Y + (visibleTop - tileY);
                    Rectangle clippedSourceRect = new Rectangle(_sourceRect.X, sourceY, _sourceRect.Width, visibleHeight);

                    // Calculate the destination rectangle to align with the parallax area
                    Rectangle destinationRect = new Rectangle(
                        _parallaxArea.Left,
                        visibleTop,
                        _parallaxArea.Width,
                        visibleHeight
                    );

                    spriteBatch.Draw(_texture, destinationRect, clippedSourceRect, Color.White);
                }
            }
        }


    }
}
