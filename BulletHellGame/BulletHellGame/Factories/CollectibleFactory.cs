using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities.Collectibles;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class CollectibleFactory
    {
        public CollectibleFactory() { }

        public Collectible CreateCollectible(CollectibleType type)
        {
            SpriteData si = null;
            switch (type)
            {
                case CollectibleType.PowerUp:
                    si = TextureManager.Instance.GetSpriteData("Collectible");
                    return new Collectible(si);
                case CollectibleType.SpeedBoost:
                    si = TextureManager.Instance.GetSpriteData("Collectible");
                    return new Collectible(si);
                case CollectibleType.ScoreBoost:
                    si = TextureManager.Instance.GetSpriteData("Collectible");
                    return new Collectible(si);
                default:
                    si = TextureManager.Instance.GetSpriteData("Collectible");
                    return new Collectible(si);
            }
        }
    }
}
