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
            SpriteInfo si = null;
            switch (type)
            {
                case BulletType.Standard:
                    si = TextureManager.Instance.GetSpriteInfo("Reimu.OrangeBullet");
                    return new Bullet(BulletType.Standard, si.Texture, position, si.Rects);
                case BulletType.Pellet:
                    si = TextureManager.Instance.GetSpriteInfo("Reimu.WhiteBullet");
                    return new Bullet(BulletType.Pellet, si.Texture, position, si.Rects);
                case BulletType.Homing:
                    si = TextureManager.Instance.GetSpriteInfo("Reimu.OrangeBullet");
                    return new Bullet(BulletType.Standard, si.Texture, position, si.Rects);
                default:
                    si = TextureManager.Instance.GetSpriteInfo("Reimu.OrangeBullet");
                    return new Bullet(BulletType.Standard, si.Texture, position, si.Rects);
            }
        }
    }
}
