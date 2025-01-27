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
                if (entity.HasComponent<PlayerInputComponent>())
                {
                    PlayerInputComponent playerInputComponent = entity.GetComponent<PlayerInputComponent>();
                    MovementComponent movementComponent = entity.GetComponent<MovementComponent>();
                    SpriteComponent spriteComponent = entity.GetComponent<SpriteComponent>();
                    SpeedComponent speedComponent = entity.GetComponent<SpeedComponent>();

                    // Update keyboard state
                    playerInputComponent.CurrentKeyboardState = Keyboard.GetState();

                    // Movement input
                    playerInputComponent.IsMovingUp = playerInputComponent.CurrentKeyboardState.IsKeyDown(Keys.W);
                    playerInputComponent.IsMovingDown = playerInputComponent.CurrentKeyboardState.IsKeyDown(Keys.S);
                    playerInputComponent.IsMovingLeft = playerInputComponent.CurrentKeyboardState.IsKeyDown(Keys.A);
                    playerInputComponent.IsMovingRight = playerInputComponent.CurrentKeyboardState.IsKeyDown(Keys.D);

                    // Action input
                    playerInputComponent.IsShooting = playerInputComponent.CurrentKeyboardState.IsKeyDown(Keys.Space);

                    // Special mode (focused mode)
                    playerInputComponent.IsFocused = playerInputComponent.CurrentKeyboardState.IsKeyDown(Keys.LeftShift);

                    // Save the current state for comparison in the next update
                    playerInputComponent.PreviousKeyboardState = playerInputComponent.CurrentKeyboardState;

                    // Handle movement based on the input states
                    Vector2 direction = Vector2.Zero;

                    if (playerInputComponent.IsMovingUp)
                        direction.Y -= 1;
                    if (playerInputComponent.IsMovingDown)
                        direction.Y += 1;
                    if (playerInputComponent.IsMovingLeft)
                        direction.X -= 1;
                    if (playerInputComponent.IsMovingRight)
                        direction.X += 1;

                    // Normalize direction and apply speed based on focused state
                    if (direction.LengthSquared() > 0)
                        direction.Normalize();

                    // Adjust speed based on whether the player is focused
                    float currentSpeed = playerInputComponent.IsFocused ? speedComponent.FocusedSpeed : speedComponent.Speed;
                    movementComponent.Velocity = direction * currentSpeed;

                    // Ensure the player stays within bounds
                    if (movementComponent.Position.X < entityManager.Bounds.Left)
                        movementComponent.Position = new Vector2(entityManager.Bounds.Left, movementComponent.Position.Y);
                    else if (movementComponent.Position.X + spriteComponent.CurrentFrame.Width > entityManager.Bounds.Right)
                        movementComponent.Position = new Vector2(entityManager.Bounds.Right - spriteComponent.CurrentFrame.Width, movementComponent.Position.Y);

                    if (movementComponent.Position.Y < entityManager.Bounds.Top)
                        movementComponent.Position = new Vector2(movementComponent.Position.X, entityManager.Bounds.Top);
                    else if (movementComponent.Position.Y + spriteComponent.CurrentFrame.Height > entityManager.Bounds.Bottom)
                        movementComponent.Position = new Vector2(movementComponent.Position.X, entityManager.Bounds.Bottom - spriteComponent.CurrentFrame.Height);
                }
            }
        }

    }
}
