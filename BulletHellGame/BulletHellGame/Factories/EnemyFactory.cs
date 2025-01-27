using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class EnemyFactory
    {
        public EnemyFactory() { }

        public Entity CreateEnemy(EnemyData enemyData)
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData("FairyBlue");
            Entity enemy = new Entity();
            enemy.AddComponent(new HitboxComponent(enemy));
            enemy.AddComponent(new HealthComponent());
            enemy.AddComponent(new SpriteComponent(spriteData));
            enemy.AddComponent(new MovementComponent());
            enemy.AddComponent(new MovementPatternComponent("zigzag"));
            return enemy;
        }
    }
}
