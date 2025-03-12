using BulletHellGame.Managers;
using BulletHellGame.Data.DataTransferObjects;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace BulletHellGame.Scenes
{
    public class GameplayScene : IScene
    {
        private ContentManager _contentManager;
        private LevelData _levelData;
        private EntityManager _entityManager;
        private SystemManager _systemManager;

        public GameplayScene(ContentManager contentManager, LevelData levelData)
        {
            _contentManager = contentManager;
        }

        public void Load()
        {
            // Load level data from a JSON file
            string levelFilePath = Path.Combine(_contentManager.RootDirectory, "Levels", "level1.json");
            _levelData = LevelManager.LoadLevel(levelFilePath);

            // Load assets like _background and music
            Texture2D background = _contentManager.Load<Texture2D>(_levelData.Background);
            SoundEffect music = _contentManager.Load<SoundEffect>(_levelData.Music);

            // Play _background music and initialize other assets
        }

        public void Update(GameTime gameTime)
        {
            /*
            // Use level data to spawn enemies at the appropriate times
            foreach (var enemy in _levelData.Enemies)
            {
                if (gameTime.TotalGameTime.TotalSeconds >= enemy.SpawnTime)
                {
                    // Spawn the enemy based on the data
                    var newEnemy = EntityManager.Instance.SpawnEnemy(enemy.Type, enemy.SpawnPosition);
                    HealthComponent health = newEnemy.GetComponent<HealthComponent>();
                    health.Heal(health.MaxHealth);
                    newEnemy.AddComponent(new MovementPatternComponent(newEnemy, enemy.MovementPattern));

                    // Handle bullet patterns
                    if (enemy.BulletPattern != null)
                    {
                        newEnemy.AddComponent(new BulletPatternComponent(newEnemy, enemy.BulletPattern.Type, enemy.BulletPattern.Speed, enemy.BulletPattern.FireRate));
                    }
                }
            }

            // Handle boss spawning
            if (_levelData.Boss != null && gameTime.TotalGameTime.TotalSeconds >= _levelData.Boss.SpawnTime)
            {
                var boss = EntityManager.Instance.SpawnBoss(_levelData.Boss.Name, _levelData.Boss.SpawnPosition);
                boss.BuildHealth(_levelData.Boss.Health);

                foreach (var phase in _levelData.Boss.Phases)
                {
                    boss.AddPhase(phase.HealthThreshold, phase.BulletPatterns);
                }
            }
            */
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _systemManager.Draw(_entityManager, spriteBatch);
        }
    }
}