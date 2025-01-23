using BulletHellGame.Data;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class BulletFactory
    {
        public BulletFactory() { }

        public Bullet CreateBullet(BulletType type, Vector2 position, Vector2 velocity)
        {
            SpriteData si = null;
            switch (type)
            {
                case BulletType.Standard:
                    si = TextureManager.Instance.GetSpriteData("Reimu.OrangeBullet");
                    return new Bullet(BulletType.Standard, si, position);
                case BulletType.Pellet:
                    si = TextureManager.Instance.GetSpriteData("Reimu.WhiteBullet");
                    return new Bullet(BulletType.Pellet, si, position);
                case BulletType.Homing:
                    si = TextureManager.Instance.GetSpriteData("Reimu.OrangeBullet");
                    return new Bullet(BulletType.Standard, si, position);
                default:
                    si = TextureManager.Instance.GetSpriteData("Reimu.OrangeBullet");
                    return new Bullet(BulletType.Standard, si, position);
            }
        }
    }
}
