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
                    return new Bullet(BulletType.Standard,TextureManager.Instance.GetTexture("Sprites/Bullets/ReimuBullet"), position, velocity);
                case BulletType.Pellet:
                    return new Bullet(BulletType.Pellet,TextureManager.Instance.GetTexture("Sprites/Bullets/ReimuPellet"), position, velocity);
                case BulletType.Homing:
                    return new HomingBullet(BulletType.Homing,TextureManager.Instance.GetTexture(""), position, velocity);
                default:
                    return new Bullet(BulletType.Standard, TextureManager.Instance.GetTexture(""), position, velocity);
            }
        }
    }
}
