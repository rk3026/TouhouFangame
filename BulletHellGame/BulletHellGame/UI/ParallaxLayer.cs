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
            int numVerticalTiles = (int)Math.Ceiling((double)_parallaxArea.Height / _sourceRect.Height) + 1;

            for (int i = 0; i < numVerticalTiles; i++)
            {
                float tileOffset = ((int)_scrollOffset % _sourceRect.Height) - _sourceRect.Height + (i * _sourceRect.Height);

                spriteBatch.Draw(
                    _texture,
                    new Rectangle(_parallaxArea.Left, (int)tileOffset, _parallaxArea.Width, _sourceRect.Height), // Stretch width
                    _sourceRect,
                    Color.White
                );
            }
        }
    }
}
