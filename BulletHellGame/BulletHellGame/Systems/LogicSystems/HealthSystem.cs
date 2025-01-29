using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class HealthSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            var entitiesToRemove = new List<Entity>();

            foreach (var entity in entityManager.GetActiveEntities())
            {
                if (entity.HasComponent<HealthComponent>())
                {
                    var healthComponent = entity.GetComponent<HealthComponent>();
                    if (healthComponent.CurrentHealth <= 0)
                    {
                        entitiesToRemove.Add(entity);
                    }
                }
            }

            // After iterating, remove entities that should be destroyed (stops the list was modified error)
            foreach (var entity in entitiesToRemove)
            {
                entityManager.QueueEntityForRemoval(entity);
            }
        }
    }
}