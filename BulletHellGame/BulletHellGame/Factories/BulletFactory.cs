using BulletHellGame.Data;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class BulletFactory
    {
        public BulletFactory() { }

        public Bullet CreateBullet(BulletType type)
        {
            SpriteData si = null;
            switch (type)
            {
                case BulletType.Standard:
                    si = TextureManager.Instance.GetSpriteData("Reimu.OrangeBullet");
                    return new Bullet(si);
                case BulletType.Pellet:
                    si = TextureManager.Instance.GetSpriteData("Reimu.WhiteBullet");
                    return new Bullet(si);
                case BulletType.Homing:
                    si = TextureManager.Instance.GetSpriteData("Reimu.OrangeBullet");
                    return new HomingBullet(si);
                default:
                    si = TextureManager.Instance.GetSpriteData("Reimu.OrangeBullet");
                    return new Bullet(si);
            }
        }
    }
}
