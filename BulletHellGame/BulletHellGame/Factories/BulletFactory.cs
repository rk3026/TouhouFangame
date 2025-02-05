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
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(bulletData.SpriteName);
            // Add the components that bullets will need:
            Entity bullet = new Entity();
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2); // For rotation, rotate at the origin
            spriteComponent.RotationSpeed = bulletData.RotationSpeed;
            bullet.AddComponent(spriteComponent);
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
