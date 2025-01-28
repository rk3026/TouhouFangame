using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;

namespace BulletHellGame.Factories
{
    public class CollectibleFactory
    {
        public CollectibleFactory() { }

        public Entity CreateCollectible(CollectibleData collectibleData)
        {
            Entity collectible = new Entity();
            collectible.AddComponent(new SpriteComponent());
            collectible.AddComponent(new MovementComponent());
            collectible.AddComponent(new HitboxComponent(collectible, 3)); // Layer 3 for collectible layer.
            collectible.AddComponent(new ScoreComponent());
            return collectible;
        }
    }
}
