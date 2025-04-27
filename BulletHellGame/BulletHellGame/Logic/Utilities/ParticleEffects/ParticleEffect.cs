namespace BulletHellGame.Logic.Utilities.ParticleEffects
{
    public class ParticleEffect
    {
        public Vector2 Position { get; set; }
        public float LifeTime { get; set; }
        public bool IsComplete => LifeTime <= 0f;
        private List<Texture2D> _frames;
        private int _currentFrameIndex;
        private float _frameDuration;
        private float _frameTimer;

        public ParticleEffect(Vector2 position, List<Texture2D> frames, float lifeTime, float frameDuration)
        {
            Position = position;
            _frames = frames;
            LifeTime = lifeTime;
            _frameDuration = frameDuration;
            _frameTimer = 0f;
            _currentFrameIndex = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsComplete)
                return;

            LifeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (LifeTime <= 0f)
            {
                LifeTime = 0f;
            }

            // Handle frame update
            _frameTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_frameTimer >= _frameDuration && _currentFrameIndex < _frames.Count - 1)
            {
                _frameTimer = 0f;
                _currentFrameIndex++;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsComplete)
                return; // Don't draw if the effect is complete

            spriteBatch.Draw(_frames[_currentFrameIndex], Position, Color.White);
        }
    }
}
