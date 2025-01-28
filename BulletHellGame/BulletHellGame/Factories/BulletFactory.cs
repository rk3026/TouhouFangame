using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;

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
            bullet.AddComponent(new PositionComponent());
            bullet.AddComponent(new VelocityComponent());
            bullet.AddComponent(new DamageComponent(bulletData.Damage));

            switch (bulletData.BulletType)
            {
                case BulletType.Standard:
                    break;
                case BulletType.Homing:
                    bullet.AddComponent(new HomingComponent());
                    break;
            }

            // Set the hitbox:
            HitboxComponent hc = new HitboxComponent(bullet, 3);
            hc.Hitbox = new Vector2(bullet.GetComponent<SpriteComponent>().CurrentFrame.Width, bullet.GetComponent<SpriteComponent>().CurrentFrame.Width);
            bullet.AddComponent(hc); // Layer 1 = enemies and their bullets

            return bullet;
        }
    }
}
