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
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(enemyData.SpriteName);
            Entity enemy = new Entity();
            enemy.AddComponent(new HealthComponent(enemyData.Health));
            enemy.AddComponent(new SpriteComponent(spriteData));
            enemy.AddComponent(new PositionComponent());
            enemy.AddComponent(new VelocityComponent());
            enemy.AddComponent(new MovementPatternComponent(enemyData.MovementPattern));

            // Setting up and adding a weapon
            BulletData bd = new BulletData();
            bd.SpriteData = TextureManager.Instance.GetSpriteData("Reimu.WhiteBullet");
            bd.Damage = 25;
            bd.BulletType = BulletType.Standard;
            WeaponComponent wc = new WeaponComponent(bd);
            wc.FireDirections.Add(new Vector2(0, 5));
            wc.FireRate = 10f;
            enemy.AddComponent(wc);

            // Set the hitbox:
            HitboxComponent hc = new HitboxComponent(enemy, 1);
            hc.Hitbox = new Vector2(enemy.GetComponent<SpriteComponent>().CurrentFrame.Width, enemy.GetComponent<SpriteComponent>().CurrentFrame.Width);
            enemy.AddComponent(hc);
            return enemy;
        }
    }
}
