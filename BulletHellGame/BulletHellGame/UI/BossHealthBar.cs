using BulletHellGame.Components;
using BulletHellGame.Entities;

namespace BulletHellGame.UI
{
    public class BossHealthBar
    {
        private Entity _boss;
        private Texture2D _barTexture;
        private Vector2 _position;
        private int _width;
        private int _height;
        private float _displayedHealthPercent;
        private float _fillSpeed = 1.0f;  // Controls the speed of the fill-up effect

        public BossHealthBar(Entity boss, Texture2D barTexture, Vector2 position, int width, int height)
        {
            _boss = boss;
            _barTexture = barTexture;
            _position = position;
            _width = width;
            _height = height;
            _displayedHealthPercent = 0f; // Start empty
        }

        public void Update(GameTime gameTime)
        {
            if (_boss == null || !_boss.HasComponent<HealthComponent>()) return;

            var healthComponent = _boss.GetComponent<HealthComponent>();
            float targetHealthPercent = (float)healthComponent.CurrentHealth / healthComponent.MaxHealth;

            // Gradually increase displayed health towards the actual health
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _displayedHealthPercent += _fillSpeed * deltaTime;
            _displayedHealthPercent = MathHelper.Clamp(_displayedHealthPercent, 0f, targetHealthPercent);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_boss == null || !_boss.HasComponent<HealthComponent>()) return;

            int healthBarWidth = (int)(_width * _displayedHealthPercent);

            // Background bar (dark red)
            spriteBatch.Draw(_barTexture, new Rectangle((int)_position.X, (int)_position.Y, _width, _height), Color.DarkRed);
            // Foreground bar (red, grows over time)
            spriteBatch.Draw(_barTexture, new Rectangle((int)_position.X, (int)_position.Y, healthBarWidth, _height), Color.Red);
        }
    }
}
