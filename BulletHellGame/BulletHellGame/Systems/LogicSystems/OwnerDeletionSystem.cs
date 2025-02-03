using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class OwnerDeletionSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(OwnerComponent)))
            {
                if (entity.TryGetComponent<OwnerComponent>(out var oc))
                {
                    if (oc.Owner == null || !oc.Owner.IsActive) // Delete entities whose owners are inactive or null
                    {
                        entityManager.QueueEntityForRemoval(entity);
                    }
                }
            }
        }
    }
}
