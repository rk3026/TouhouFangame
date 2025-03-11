//using BulletHellGame.Components;
//using BulletHellGame.Entities;

//namespace BulletHellGame.Commands
//{
//    public class ShootCommand : ICommand
//    {
//        private Entity _entity;
//        public ShootCommand(Entity entity)
//        {
//            _entity = entity;
//        }

//        public void Execute()
//        {
//            if (_entity.TryGetComponent<ShootingComponent>(out var sc) &&
//                _entity.TryGetComponent<PositionComponent>(out var pc) &&
//                _entity.TryGetComponent<HitboxComponent>(out var hc))
//            {
//                foreach (var weapon in sc.Weapons)
//                {
//                    if (sc.WeaponCooldowns.TryGetValue(weapon, out float cooldown) && cooldown >= weapon.FireRate)
//                    {
//                        foreach (var firingDirection in weapon.FireDirections)
//                        {
//                            // Spawn the bullet
//                            entityManager.SpawnBullet(
//                                weapon.BulletData,
//                                pc.Position,
//                                hc.Layer,
//                                firingDirection,
//                                _entity // owner of the bullet
//                            );
//                        }

//                        // Reset cooldown after firing
//                        sc.WeaponCooldowns[weapon] = 0f;
//                    }
//                }
//            }
//        }
//    }

//}
