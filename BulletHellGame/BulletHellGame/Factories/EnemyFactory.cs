using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System;

namespace BulletHellGame.Factories
{
    public class EnemyFactory
    {
        private Random _random = new Random();

        public EnemyFactory() { }

        public Entity CreateEnemy(EnemyData enemyData)
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(enemyData.SpriteName);
            Entity enemy = new Entity();
            enemy.AddComponent(new HealthComponent(enemyData.Health));
            enemy.AddComponent(new SpriteComponent(spriteData));
            enemy.AddComponent(new PositionComponent());
            enemy.AddComponent(new VelocityComponent());
            enemy.AddComponent(new MovementPatternComponent(enemyData.MovementPattern));

            // Setting up and adding a weapon
            BulletData bd = new BulletData();
            bd.SpriteName = "Reimu.WhiteBullet";
            bd.Damage = _random.Next(20, 30);  // Randomized damage between 20-30
            bd.BulletType = BulletType.Standard;

            ShootingComponent wc = new ShootingComponent(bd);
            wc.FireRate = (float)_random.NextDouble() * 5f + 5f; // Random fire rate between 5-10 shots per second
            wc.FireDirections.Add(new Vector2(_random.Next(-2, 3), _random.Next(1, 4))); // Random direction within a range

            enemy.AddComponent(wc);

            // Set the hitbox:
            HitboxComponent hc = new HitboxComponent(enemy, 1);
            float spriteWidth = enemy.GetComponent<SpriteComponent>().CurrentFrame.Width;
            hc.Hitbox = new Vector2(spriteWidth, spriteWidth);
            enemy.AddComponent(hc);

            return enemy;
        }
    }
}
