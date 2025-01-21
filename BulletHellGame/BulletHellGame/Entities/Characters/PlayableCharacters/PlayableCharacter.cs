using BulletHellGame.Components;
using BulletHellGame.Entities.Characters;
using BulletHellGame.Managers;

public class PlayableCharacter : Character
{
    private static readonly float MOVE_SPEED = 500f;

    public PlayableCharacter(Texture2D texture, Vector2 position, List<Rectangle> frameRects = null, double frameDuration = 0.1, bool isAnimating = false) : base(texture, position, frameRects, frameDuration, isAnimating)
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
        if (InputManager.KeyDown(Keys.S) && Position.Y <= Globals.WindowSize.Y - this.SourceRect.Value.Height)
        {
            movementDirection.Y += 1;
        }
        if (InputManager.KeyDown(Keys.A) && Position.X >= 0)
        {
            movementDirection.X -= 1;
        }
        if (InputManager.KeyDown(Keys.D) && Position.X <= Globals.WindowSize.X - this.SourceRect.Value.Width)
        {
            movementDirection.X += 1;
        }


        // Update the velocity in the MovementComponent
        var movementComponent = this.GetComponent<MovementComponent>();
        movementComponent.Velocity = movementDirection * MOVE_SPEED;

        // Check if moving:
        /*
        if (movementComponent.Velocity != Vector2.Zero)
        {
            if (movementComponent.Velocity.X > 0) // Moving right
            {
                // Set the texture for moving right
                this._frameRects = TextureManager.Instance.GetSpriteInfo("Reimu.MoveRight").Rects;

                // Make sure the sprite is not flipped when moving right
                var spriteEffectComponent = this.GetComponent<SpriteEffectComponent>();
                spriteEffectComponent.spriteEffects.Add(SpriteEffects.FlipHorizontally); // No flip
            }
            else // Moving left
            {
                // Set the texture for moving left
                this._frameRects = TextureManager.Instance.GetSpriteInfo("Reimu.MoveLeft").Rects;

                // Flip the sprite horizontally when moving left
                var spriteEffectComponent = this.GetComponent<SpriteEffectComponent>();
                spriteEffectComponent.Effect = SpriteEffect.FlipHorizontally; // Flip the sprite
            }
        }
        */


        // Handle shooting input: if Space is pressed, call the Shoot method on the WeaponComponent
        if (InputManager.KeyDown(Keys.Space))
        {
            var weaponComponent = this.GetComponent<WeaponComponent>();
            weaponComponent.Shoot();
        }

        base.Update(gameTime); // Ensure components are updated
    }
}
