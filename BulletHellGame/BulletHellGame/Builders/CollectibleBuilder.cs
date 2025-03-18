using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;

namespace BulletHellGame.Builders
{
    public class CollectibleBuilder : EntityBuilder<CollectibleData>
    {
        public CollectibleBuilder() : base() { }
        public CollectibleBuilder(CollectibleData data) : base(data) { }

        public override void BuildSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            _entity.AddComponent(spriteComponent);
        }

        public override void BuildPosition()
        {
            _entity.AddComponent(new PositionComponent());
        }

        public override void BuildVelocity()
        {
            _entity.AddComponent(new VelocityComponent());
        }

        public override void BuildAttractable()
        {
            _entity.AddComponent(new AttractableComponent());
        }

        public override void BuildPickupEffect()
        {
            PickUpEffectComponent pickUpEffectComponent = new PickUpEffectComponent();
            foreach (var effect in _entityData.Effects)
            {
                pickUpEffectComponent.Effects.Add(effect.Key, effect.Value);
            }
            _entity.AddComponent(pickUpEffectComponent);
        }

        public override void BuildHitbox()
        {
            SpriteComponent sprite = _entity.GetComponent<SpriteComponent>();
            if (sprite != null)
            {
                HitboxComponent hitbox = new HitboxComponent(_entity, 3)
                {
                    Hitbox = new Vector2(sprite.CurrentFrame.Width, sprite.CurrentFrame.Height)
                };
                _entity.AddComponent(hitbox);
            }
        }
    }
}
