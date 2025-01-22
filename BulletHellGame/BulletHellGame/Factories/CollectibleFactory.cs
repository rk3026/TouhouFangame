using BulletHellGame.Data;
using BulletHellGame.Entities.Collectibles;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class CollectibleFactory
    {
        public CollectibleFactory() { }

        public Collectible CreateCollectible(CollectibleType type, Vector2 position)
        {
            SpriteData si = null;
            switch (type)
            {
                case CollectibleType.PowerUp:
                    si = TextureManager.Instance.GetSpriteInfo("Collectible");
                    return new Collectible(si, position);
                case CollectibleType.SpeedBoost:
                    si = TextureManager.Instance.GetSpriteInfo("Collectible");
                    return new Collectible(si, position);
                case CollectibleType.ScoreBoost:
                    si = TextureManager.Instance.GetSpriteInfo("Collectible");
                    return new Collectible(si, position);
                default:
                    si = TextureManager.Instance.GetSpriteInfo("Collectible");
                    return new Collectible(si, position);
            }
        }
    }
}
