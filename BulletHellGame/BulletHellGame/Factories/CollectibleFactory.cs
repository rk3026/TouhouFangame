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
            collectible.AddComponent(new PositionComponent());
            collectible.AddComponent(new VelocityComponent());
            collectible.AddComponent(new ScoreComponent());
            
            // Set the hitbox:
            HitboxComponent hc = new HitboxComponent(collectible, 3);
            hc.Hitbox = new Vector2(collectible.GetComponent<SpriteComponent>().CurrentFrame.Width, collectible.GetComponent<SpriteComponent>().CurrentFrame.Width);
            collectible.AddComponent(hc);
            return collectible;
        }
    }
}
