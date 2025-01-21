using BulletHellGame.Data;
using BulletHellGame.Entities.Bullets;
using BulletHellGame.Entities.Collectibles;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class CollectibleFactory
    {
        public CollectibleFactory() { }

        public Collectible CreateCollectible(CollectibleType type, Vector2 position)
        {
            SpriteInfo si = null;
            switch (type)
            {
                case CollectibleType.PowerUp:
                    si = TextureManager.Instance.GetSpriteInfo("Collectible");
                    return new Collectible(si.Texture, position, si.Rects);
                case CollectibleType.SpeedBoost:
                    si = TextureManager.Instance.GetSpriteInfo("Collectible");
                    return new Collectible(si.Texture, position, si.Rects);
                case CollectibleType.ScoreBoost:
                    si = TextureManager.Instance.GetSpriteInfo("Collectible");
                    return new Collectible(si.Texture, position, si.Rects);
                default:
                    si = TextureManager.Instance.GetSpriteInfo("Collectible");
                    return new Collectible(si.Texture, position, si.Rects);
            }
        }
    }
}
