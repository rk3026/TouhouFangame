using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class PlayerInputSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(PlayerInputComponent)))
            {
                if (entity.TryGetComponent<PlayerInputComponent>(out var pic) &&
                    entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<VelocityComponent>(out var vc) &&
                    entity.TryGetComponent<SpriteComponent>(out var sc) &&
                    entity.TryGetComponent<SpeedComponent>(out var speedComponent)
                    )
                {
                    // Movement input
                    pic.IsMovingUp = InputManager.Instance.ActionDown(GameAction.MoveUp);
                    pic.IsMovingDown = InputManager.Instance.ActionDown(GameAction.MoveDown);
                    pic.IsMovingLeft = InputManager.Instance.ActionDown(GameAction.MoveLeft);
                    pic.IsMovingRight = InputManager.Instance.ActionDown    (GameAction.MoveRight);

                    // Action input
                    pic.IsShooting = InputManager.Instance.ActionDown(GameAction.Shoot);

                    // Special mode (focused mode)
                    pic.IsFocused = InputManager.Instance.ActionDown(GameAction.Slow);

                    // Handle movement based on the input states
                    Vector2 direction = Vector2.Zero;

                    if (pic.IsMovingUp)
                        direction.Y -= 1;
                    if (pic.IsMovingDown)
                        direction.Y += 1;
                    if (pic.IsMovingLeft)
                        direction.X -= 1;
                    if (pic.IsMovingRight)
                        direction.X += 1;

                    // Normalize direction and apply speed based on focused state
                    if (direction.LengthSquared() > 0)
                        direction.Normalize();

                    // Adjust speed based on whether the player is focused
                    float currentSpeed = pic.IsFocused ? speedComponent.FocusedSpeed : speedComponent.Speed;
                    vc.Velocity = direction * currentSpeed;

                    // Ensure the player stays within bounds
                    float halfWidth = sc.CurrentFrame.Width / 2f;
                    float halfHeight = sc.CurrentFrame.Height / 2f;

                    if (pc.Position.X - halfWidth < entityManager.Bounds.Left)
                        pc.Position = new Vector2(entityManager.Bounds.Left + halfWidth, pc.Position.Y);
                    else if (pc.Position.X + halfWidth > entityManager.Bounds.Right)
                        pc.Position = new Vector2(entityManager.Bounds.Right - halfWidth, pc.Position.Y);

                    if (pc.Position.Y - halfHeight < entityManager.Bounds.Top)
                        pc.Position = new Vector2(pc.Position.X, entityManager.Bounds.Top + halfHeight);
                    else if (pc.Position.Y + halfHeight > entityManager.Bounds.Bottom)
                        pc.Position = new Vector2(pc.Position.X, entityManager.Bounds.Bottom - halfHeight);
                }
            }
        }

    }
}
