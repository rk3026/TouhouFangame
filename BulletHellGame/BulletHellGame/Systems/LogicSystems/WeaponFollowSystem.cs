﻿using BulletHellGame.Components;
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

                if (ownerComponent.Owner.TryGetComponent<PositionComponent>(out var ownerPos))
                {

                    weaponPosition.Position = ownerPos.Position + ownerComponent.Offset;
                }
            }
        }
    }
}
