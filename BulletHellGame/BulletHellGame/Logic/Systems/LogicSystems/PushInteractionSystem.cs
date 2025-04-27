using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.LogicSystems
{
    public class PushInteractionSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            // Get all pushers with required components
            var pushers = entityManager.GetEntitiesWithComponents(
                typeof(PusherComponent),
                typeof(PositionComponent)
            );

            // Get all pushables with required components
            var pushables = entityManager.GetEntitiesWithComponents(
                typeof(PushableComponent),
                typeof(PositionComponent),
                typeof(VelocityComponent)
            );

            foreach (var pusher in pushers)
            {
                var pusherC = pusher.GetComponent<PusherComponent>();
                var pusherPos = pusher.GetComponent<PositionComponent>().Position;

                foreach (var pushable in pushables)
                {
                    // Avoid self-push
                    if (pushable == pusher)
                        continue;

                    var pushablePos = pushable.GetComponent<PositionComponent>().Position;
                    float distance = Vector2.Distance(pusherPos, pushablePos);

                    // Check within push radius
                    if (distance <= pusherC.PushRadius)
                    {
                        var direction = Vector2.Normalize(pushablePos - pusherPos);
                        var resistance = pushable.GetComponent<PushableComponent>().PushResistance;
                        var velocity = pushable.GetComponent<VelocityComponent>();

                        float force = pusherC.PushPower / resistance;
                        velocity.Velocity += direction * force;
                    }
                }
            }
        }
    }
}
