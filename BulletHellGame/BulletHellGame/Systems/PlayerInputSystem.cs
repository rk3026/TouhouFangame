using BulletHellGame.Components;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System.Linq;

namespace BulletHellGame.Systems
{
    public class PlayerInputSystem : ISystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            List<Entity> entities = entityManager.GetActiveEntities().ToList();

            foreach (Entity entity in entities)
            {
                if (entity.TryGetComponent<PlayerInputComponent>(out var pic) &&
                    entity.TryGetComponent<PositionComponent>(out var pc) &&
                    entity.TryGetComponent<VelocityComponent>(out var vc) &&
                    entity.TryGetComponent<SpriteComponent>(out var sc) &&
                    entity.TryGetComponent<SpeedComponent>(out var speedComponent)
                    )
                {
                    // Update keyboard state
                    pic.CurrentKeyboardState = Keyboard.GetState();

                    // Movement input
                    pic.IsMovingUp = pic.CurrentKeyboardState.IsKeyDown(Keys.W);
                    pic.IsMovingDown = pic.CurrentKeyboardState.IsKeyDown(Keys.S);
                    pic.IsMovingLeft = pic.CurrentKeyboardState.IsKeyDown(Keys.A);
                    pic.IsMovingRight = pic.CurrentKeyboardState.IsKeyDown(Keys.D);

                    // Action input
                    pic.IsShooting = pic.CurrentKeyboardState.IsKeyDown(Keys.Space);

                    // Special mode (focused mode)
                    pic.IsFocused = pic.CurrentKeyboardState.IsKeyDown(Keys.LeftShift);

                    // Save the current state for comparison in the next update
                    pic.PreviousKeyboardState = pic.CurrentKeyboardState;

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
                    if (pc.Position.X < entityManager.Bounds.Left)
                        pc.Position = new Vector2(entityManager.Bounds.Left, pc.Position.Y);
                    else if (pc.Position.X + sc.CurrentFrame.Width > entityManager.Bounds.Right)
                        pc.Position = new Vector2(entityManager.Bounds.Right - sc.CurrentFrame.Width, pc.Position.Y);

                    if (pc.Position.Y < entityManager.Bounds.Top)
                        pc.Position = new Vector2(pc.Position.X, entityManager.Bounds.Top);
                    else if (pc.Position.Y + sc.CurrentFrame.Height > entityManager.Bounds.Bottom)
                        pc.Position = new Vector2(pc.Position.X, entityManager.Bounds.Bottom - sc.CurrentFrame.Height);
                }
            }
        }

    }
}
