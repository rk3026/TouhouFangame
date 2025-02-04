using BulletHellGame.Components;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class WeaponFollowSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (var weapon in entityManager.GetEntitiesWithComponents(typeof(OwnerComponent), typeof(PositionComponent), typeof(ShootingComponent)))
            {
                var ownerComponent = weapon.GetComponent<OwnerComponent>();
                var weaponPosition = weapon.GetComponent<PositionComponent>();

                if (ownerComponent.Owner.TryGetComponent<PositionComponent>(out var ownerPos) &&
                    ownerComponent.Owner.TryGetComponent<SpriteComponent>(out var ownerSprite))
                {
                    Vector2 ownerCenterOffset = new Vector2(ownerSprite.CurrentFrame.Width / 2, ownerSprite.CurrentFrame.Height / 2);
                    weapon.TryGetComponent<SpriteComponent>(out var weaponSprite);

                    Vector2 topLeft = ownerPos.Position - weaponSprite.SpriteData.Origin;
                    Vector2 center = topLeft + ownerCenterOffset;
                    weaponPosition.Position = center + ownerComponent.Offset;
                }
            }
        }
    }
}
