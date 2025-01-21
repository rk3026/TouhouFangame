using BulletHellGame.Entities.Bullets;
using BulletHellGame.Entities.Collectibles;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class CollectibleFactory
    {
        public CollectibleFactory() { }

        public Collectible CreateCollectible(CollectibleType type, Vector2 position, Vector2 velocity)
        {
            switch (type)
            {
                case CollectibleType.PowerUp:
                    return new Collectible(TextureManager.Instance.GetTexture("Sprites/Bullets/ReimuPellet"), position, velocity);
                case CollectibleType.SpeedBoost:
                    return new Collectible(TextureManager.Instance.GetTexture("Sprites/Bullets/ReimuPellet"), position, velocity);
                case CollectibleType.ScoreBoost:
                    return new Collectible(TextureManager.Instance.GetTexture("Sprites/Bullets/ReimuPellet"), position, velocity);
                default:
                    return new Collectible(TextureManager.Instance.GetTexture("Sprites/Bullets/ReimuPellet"), position, velocity);
            }
        }
    }
}
