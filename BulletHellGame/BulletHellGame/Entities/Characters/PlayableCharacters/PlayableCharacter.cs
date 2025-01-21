using BulletHellGame.Components;
using BulletHellGame.Entities.Characters;

public class PlayableCharacter : Character
{
    private static readonly float MOVE_SPEED = 500f;

    public PlayableCharacter(Texture2D texture, Vector2 position, Vector2 velocity) : base(texture, position, velocity)
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
        if (InputManager.KeyDown(Keys.S) && Position.Y <= Globals.WindowSize.Y - this.Texture.Height) //  * this.Texture.GraphicsDevice. ?
        {
            movementDirection.Y += 1;
        }
        if (InputManager.KeyDown(Keys.A) && Position.X >= 0)
        {
            movementDirection.X -= 1;
        }
        if (InputManager.KeyDown(Keys.D) && Position.X <= Globals.WindowSize.X - this.Texture.Width)
        {
            movementDirection.X += 1;
        }


        // Update the velocity in the MovementComponent
        var movementComponent = this.GetComponent<MovementComponent>();
        movementComponent.Velocity = movementDirection * MOVE_SPEED;

        // Handle shooting input: if Space is pressed, call the Shoot method on the WeaponComponent
        if (InputManager.KeyDown(Keys.Space))
        {
            var weaponComponent = this.GetComponent<WeaponComponent>();
            weaponComponent.Shoot();
        }

        base.Update(gameTime); // Ensure components are updated
    }
}
