using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Factories
{
    public class CollectibleFactory
    {
        public CollectibleFactory() { }

        public Entity CreateCollectible(CollectibleData collectibleData)
        {
            Entity collectible = new Entity();
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(collectibleData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            collectible.AddComponent(spriteComponent);
            collectible.AddComponent(new PositionComponent());
            collectible.AddComponent(new VelocityComponent());
            collectible.AddComponent(new AttractableComponent());

            // Adding the effects of the collectible when picked up:
            PickUpEffectComponent pickUpEffectComponent = new PickUpEffectComponent();
            foreach (var effect in collectibleData.Effects)
            {
                pickUpEffectComponent.Effects.Add(effect.Key, effect.Value);
            }

            collectible.AddComponent(pickUpEffectComponent);

            // Set the hitbox:
            HitboxComponent hc = new HitboxComponent(collectible, 3);
            hc.Hitbox = new Vector2(collectible.GetComponent<SpriteComponent>().CurrentFrame.Width, collectible.GetComponent<SpriteComponent>().CurrentFrame.Width);
            collectible.AddComponent(hc);
            return collectible;
        }
    }
}
