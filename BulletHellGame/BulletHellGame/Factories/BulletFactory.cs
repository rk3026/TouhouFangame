using BulletHellGame.Entities;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class BulletFactory
    {
        public BulletFactory() { }

        public Bullet CreateBullet(BulletType type, Vector2 position, Vector2 velocity)
        {
            switch (type)
            {
                case BulletType.Standard:
                    return new Bullet(TextureManager.Instance.GetTexture("Sprites/ReimuBullet"), position, velocity);
                case BulletType.Pellet:
                    return new Bullet(TextureManager.Instance.GetTexture("Sprites/ReimuPellet"), position, velocity);
                case BulletType.Homing:
                    return new HomingBullet(TextureManager.Instance.GetTexture(""), position, velocity);
                default:
                    return new Bullet(TextureManager.Instance.GetTexture(""), position, velocity);
            }
        }
    }
}
