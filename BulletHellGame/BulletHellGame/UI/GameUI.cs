using BulletHellGame.Managers;
using BulletHellGame.Components;
using BulletHellGame.Entities;

namespace BulletHellGame.UI
{
    public class GameUI
    {
        private SpriteFont _font;
        private Rectangle _uiArea;
        private EntityManager _entityManager;
        private float _elapsedTime;

        public GameUI(SpriteFont font, Rectangle uiArea, EntityManager entityManager)
        {
            _font = font;
            _uiArea = uiArea;
            _entityManager = entityManager;
            _elapsedTime = 0f;
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(_uiArea.Left + 10, _uiArea.Top + 10);

            spriteBatch.DrawString(_font, $"Bullets: {_entityManager.GetEntityCount(EntityType.Bullet)}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(_font, $"Enemies: {_entityManager.GetEntityCount(EntityType.Enemy)}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(_font, $"Collectibles: {_entityManager.GetEntityCount(EntityType.Collectible)}", position, Color.White);
            position.Y += 20;
            spriteBatch.DrawString(_font, $"Players: {_entityManager.GetEntityCount(EntityType.Player)}", position, Color.White);
            position.Y += 20;

            // DrawActiveShader elapsed stage time (formatted as MM:SS)
            TimeSpan timeSpan = TimeSpan.FromSeconds(_elapsedTime);
            string timeString = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            spriteBatch.DrawString(_font, $"Time: {timeString}", position, Color.White);
            position.Y += 20;

            // Display Player Stats
            foreach (Entity player in _entityManager.GetEntitiesWithComponents(typeof(PlayerStatsComponent)))
            {
                var stats = player.GetComponent<PlayerStatsComponent>();
                if (stats != null)
                {
                    spriteBatch.DrawString(_font, $"Score: {stats.Score}", position, Color.White);
                    position.Y += 20;
                    spriteBatch.DrawString(_font, $"Lives: {stats.Lives}", position, Color.White);
                    position.Y += 20;
                    spriteBatch.DrawString(_font, $"Bombs: {stats.Bombs}", position, Color.White);
                    position.Y += 20;
                    spriteBatch.DrawString(_font, $"Power: {stats.Power}", position, Color.White);
                    position.Y += 20;
                    spriteBatch.DrawString(_font, $"Power Level: {stats.CurrentPowerLevel}", position, Color.White);
                    position.Y += 20;
                    spriteBatch.DrawString(_font, $"Cherry Points: {stats.CherryPoints}", position, Color.White);
                }
            }
        }
    }
}
