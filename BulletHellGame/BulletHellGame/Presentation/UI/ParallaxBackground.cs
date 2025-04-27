namespace BulletHellGame.Presentation.UI
{
    public class ParallaxBackground
    {
        private List<ParallaxLayer> _layers = new List<ParallaxLayer>();

        public void AddLayer(Texture2D texture, Rectangle sourceRect, Rectangle parallaxArea, float speed)
        {
            _layers.Add(new ParallaxLayer(texture, sourceRect, parallaxArea, speed));
        }

        public void RemoveLayer(int layer)
        {
            _layers.RemoveAt(layer);
        }

        public void ClearLayers()
        {
            _layers.Clear();
        }

        public void Update(GameTime gameTime, SpriteEffects se = SpriteEffects.None)
        {
            foreach (var layer in _layers)
                layer.Update(gameTime, se);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects se = SpriteEffects.None)
        {
            foreach (var layer in _layers)
                layer.Draw(spriteBatch, se);
        }
    }
}
