using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class BulletFactory
    {
        public BulletFactory() { }

        public Entity CreateBullet(BulletData bulletData)
        {
            SpriteData spriteData = bulletData.SpriteData;
            // Add the components that bullets will need:
            Entity bullet = new Entity();
            bullet.AddComponent(new SpriteComponent(spriteData));
            bullet.AddComponent(new HitboxComponent(bullet));
            bullet.AddComponent(new MovementComponent());
            bullet.AddComponent(new DamageComponent(bulletData.Damage));

            switch (bulletData.BulletType)
            {
                case BulletType.Standard:
                    break;
                case BulletType.Homing:
                    bullet.AddComponent(new HomingComponent());
                    break;
            }

            return bullet;
        }
    }
}
