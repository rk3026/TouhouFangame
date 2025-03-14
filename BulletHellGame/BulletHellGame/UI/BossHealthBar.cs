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

        public BossHealthBar(Entity boss, Texture2D barTexture, Vector2 position, int width, int height)
        {
            _boss = boss;
            _barTexture = barTexture;
            _position = position;
            _width = width;
            _height = height;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_boss == null || !_boss.HasComponent<HealthComponent>()) return;

            var healthComponent = _boss.GetComponent<HealthComponent>();
            float healthPercent = (float)healthComponent.CurrentHealth / healthComponent.MaxHealth;
            int healthBarWidth = (int)(_width * healthPercent);

            spriteBatch.Draw(_barTexture, new Rectangle((int)_position.X, (int)_position.Y, _width, _height), Color.DarkRed);
            spriteBatch.Draw(_barTexture, new Rectangle((int)_position.X, (int)_position.Y, healthBarWidth, _height), Color.Red);
        }
    }
}
