using BulletHellGame.Components;
using BulletHellGame.Data;
using BulletHellGame.Entities.Characters;
using BulletHellGame.Managers;
using SharpFont.PostScript;

public class PlayableCharacter : Character
{
    private static readonly float MOVE_SPEED = 500f;

    public PlayableCharacter(SpriteData spriteData, Vector2 position) : base(spriteData, position)
    {
    }

    public override void Update(GameTime gameTime)
    {
        // Update the input manager to get the latest keyboard state
        InputManager.Update();

        // Get the current movement direction based on WASD keys
        Vector2 movementDirection = Vector2.Zero;

        // Use the InputManager to check if keys are pressed
        if (InputManager.KeyDown(Keys.W) && Position.Y >= 0)
        {
            movementDirection.Y -= 1;
        }
        if (InputManager.KeyDown(Keys.S) && Position.Y <= Globals.WindowSize.Y - this.GetComponent<SpriteComponent>().CurrentFrame.Height)
        {
            movementDirection.Y += 1;
        }
        if (InputManager.KeyDown(Keys.A) && Position.X >= 0)
        {
            movementDirection.X -= 1;
        }
        if (InputManager.KeyDown(Keys.D) && Position.X <= Globals.WindowSize.X - this.GetComponent<SpriteComponent>().CurrentFrame.Width)
        {
            movementDirection.X += 1;
        }

        // Normalize the movement direction to avoid faster diagonal movement
        if (movementDirection.Length() > 0)
        {
            movementDirection.Normalize();  // Normalize the vector to a unit length
        }

        // Update the velocity in the MovementComponent (scaled by MOVE_SPEED)
        var movementComponent = this.GetComponent<MovementComponent>();
        movementComponent.Velocity = movementDirection * MOVE_SPEED;

        this.UpdateAnimation();

        // Handle shooting input: if Space is pressed, call the Shoot method on the WeaponComponent
        if (InputManager.KeyDown(Keys.Space))
        {
            var weaponComponent = this.GetComponent<WeaponComponent>();
            weaponComponent.Shoot();
        }

        base.Update(gameTime); // Ensure components are updated
    }

    private void UpdateAnimation()
    {
        // Get the movement component to track the velocity
        MovementComponent movementComponent = this.GetComponent<MovementComponent>();
        float speedX = movementComponent.Velocity.X; // Get the horizontal speed (X component of velocity)

        // Get the sprite component for changing the animation
        SpriteComponent spriteComponent = this.GetComponent<SpriteComponent>();

        // Only change the animation based on horizontal movement (X velocity)
        if (speedX != 0) // Check if the player is moving left or right
        {
            // Calculate the frame to display based on the horizontal speed (X)
            int speedFrame = (int)(Math.Abs(speedX) / 100);  // You can adjust the divisor (e.g., 100) for better control
            int maxFrames = spriteComponent.SpriteInfo.Animations["MoveLeft"].Count;  // Total frames in the "MoveLeft" animation

            // Ensure the frame doesn't exceed the total number of frames
            speedFrame = Math.Min(speedFrame, maxFrames - 1); // Avoid going beyond the last frame

            // Switch the animation to "MoveLeft" and set the frame based on horizontal speed
            spriteComponent.SwitchAnimation("MoveLeft");
            spriteComponent.SetFrameIndex(speedFrame); // This method will update the frame index dynamically

            // Optional: Flip sprite horizontally if moving right
            if (speedX > 0)
            {
                spriteComponent.SpriteEffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteComponent.SpriteEffect = SpriteEffects.None;
            }
        }
        else
        {
            // If not moving horizontally, switch to idle animation
            spriteComponent.SwitchAnimation("Idle");
            spriteComponent.SpriteEffect = SpriteEffects.None;
        }
    }


}
