using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Systems;

namespace BulletHellGame.Logic.Systems.LogicSystems
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
