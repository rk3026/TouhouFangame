using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System;

namespace BulletHellGame.Factories
{
    public class EnemyFactory
    {
        public EnemyFactory() { }

        public Entity CreateEnemy(EnemyData enemyData)
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(enemyData.SpriteName);
            Entity enemy = new Entity();
            enemy.AddComponent(new HealthComponent(enemyData.Health));
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            enemy.AddComponent(spriteComponent);
            enemy.AddComponent(new PositionComponent());
            enemy.AddComponent(new VelocityComponent());
            enemy.AddComponent(new MovementPatternComponent(enemyData.MovementPattern));

            ShootingComponent shc = new ShootingComponent(enemyData.Weapons);
            enemy.AddComponent(shc);

            // Attach Loot:
            LootComponent lc = new LootComponent();
            foreach(CollectibleData cd in enemyData.Loot)
            {
                lc.Loot.Add(cd);
            }
            enemy.AddComponent(lc);

            // Set the hitbox:
            HitboxComponent hc = new HitboxComponent(enemy, 1);
            float spriteWidth = enemy.GetComponent<SpriteComponent>().CurrentFrame.Width;
            float spriteHeight = enemy.GetComponent<SpriteComponent>().CurrentFrame.Height;
            hc.Hitbox = new Vector2(spriteWidth, spriteHeight);
            enemy.AddComponent(hc);

            return enemy;
        }
    }
}
