using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Managers;

namespace BulletHellGame.Builders
{
    public class CollectibleBuilder : EntityBuilder<CollectibleData>
    {
        public CollectibleBuilder() : base() { }
        public CollectibleBuilder(CollectibleData data) : base(data) { }

        public override void SetSprite()
        {
            SpriteData spriteData = TextureManager.Instance.GetSpriteData(_entityData.SpriteName);
            SpriteComponent spriteComponent = new SpriteComponent(spriteData);
            spriteComponent.SpriteData.Origin = new Vector2(spriteComponent.CurrentFrame.Width / 2, spriteComponent.CurrentFrame.Height / 2);
            _entity.AddComponent(spriteComponent);
        }

        public override void SetPosition()
        {
            _entity.AddComponent(new PositionComponent());
        }

        public override void SetVelocity()
        {
            _entity.AddComponent(new VelocityComponent());
        }

        public override void SetAttractable()
        {
            _entity.AddComponent(new AttractableComponent());
        }

        public override void SetPickupEffect()
        {
            PickUpEffectComponent pickUpEffectComponent = new PickUpEffectComponent();
            foreach (var effect in _entityData.Effects)
            {
                pickUpEffectComponent.Effects.Add(effect.Key, effect.Value);
            }
            _entity.AddComponent(pickUpEffectComponent);
        }

        public override void SetHitbox()
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
