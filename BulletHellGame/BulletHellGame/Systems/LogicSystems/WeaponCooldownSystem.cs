//using BulletHellGame.Components;
//using BulletHellGame.Managers;
//using BulletHellGame.Systems;

//public class WeaponCooldownSystem : ILogicSystem
//{
//    public void Update(EntityManager entityManager, GameTime gameTime)
//    {
//        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

//        foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(ShootingComponent)))
//        {
//            var shooting = entity.GetComponent<ShootingComponent>();

//            // Update cooldowns for each weapon
//            foreach (var weapon in shooting.Weapons)
//            {
//                if (shooting.WeaponCooldowns.ContainsKey(weapon))
//                {
//                    shooting.WeaponCooldowns[weapon] += deltaTime;  // Update cooldown
//                }
//            }
//        }
//    }
//}
