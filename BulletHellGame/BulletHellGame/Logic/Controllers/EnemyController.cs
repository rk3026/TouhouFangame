using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Controllers
{
    public class EnemyController : EntityController
    {
        public override void Update(EntityManager entityManager, Entity entity)
        {
            this.IsShooting = true; // Constantly trying to shoot

            PositionComponent pc = entity.GetComponent<PositionComponent>();
            VelocityComponent vc = entity.GetComponent<VelocityComponent>();
            Rectangle emBounds = entityManager.Bounds;
            Rectangle enemyBounds = new Rectangle(emBounds.X, emBounds.Y, emBounds.Width, emBounds.Height / 2);

            // Check Y bounds
            if (pc.Position.Y <= enemyBounds.Top || pc.Position.Y >= enemyBounds.Bottom)
            {
                vc.Velocity = new Vector2(vc.Velocity.X, -vc.Velocity.Y);
                // Optional: clamp the position back into bounds
                pc.Position = new Vector2(pc.Position.X, Math.Clamp(pc.Position.Y, enemyBounds.Top, enemyBounds.Bottom));
            }

            // Check X bounds
            if (pc.Position.X <= enemyBounds.Left || pc.Position.X >= enemyBounds.Right)
            {
                vc.Velocity = new Vector2(-vc.Velocity.X, vc.Velocity.Y);
                // Optional: clamp the position back into bounds
                pc.Position = new Vector2(Math.Clamp(pc.Position.X, enemyBounds.Left, enemyBounds.Right), pc.Position.Y);
            }
        }
    }
}
