using BulletHellGame.Components;
using BulletHellGame.Data;
using BulletHellGame.Entities.Characters;
using BulletHellGame.Managers;
using SharpFont.PostScript;
using System.Linq;

public class PlayableCharacter : Character
{
    private static readonly float MOVE_SPEED = 500f;
    private List<WeaponComponent> _weapons = new List<WeaponComponent>();

    public PlayableCharacter(SpriteData spriteData, Vector2 position) : base(spriteData, position)
    {
        // Get the sprite component
        SpriteData weaponSpriteData = TextureManager.Instance.GetSpriteData("Reimu.YinYangOrb");
        Rectangle weaponSprite = weaponSpriteData.Animations["Default"].First<Rectangle>();
        SpriteComponent spriteComponent = this.GetComponent<SpriteComponent>();
        Rectangle characterRect = spriteComponent.CurrentFrame;

        // Add another weapon to the right side of the character
        int WEAPON_OFFSET = 6;
        _weapons.Add(new WeaponComponent(this, weaponSpriteData, new Vector2(WEAPON_OFFSET + characterRect.Width, characterRect.Height / 2 -weaponSprite.Height)));

        // Add a weapon to the left side of the character
        _weapons.Add(new WeaponComponent(this, weaponSpriteData, new Vector2(-weaponSprite.Width - WEAPON_OFFSET, characterRect.Height / 2 -weaponSprite.Height)));

        // Add the weapons to the character's component list
        foreach (var weapon in _weapons)
        {
            AddComponent(weapon);
        }
    }


    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        foreach (var weapon in _weapons)
        {
            weapon.Draw(spriteBatch);
        }
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
            foreach (var weapon in _weapons)
            {
                weapon.Shoot(new List<Vector2> { Vector2.UnitY * -1 }); // Shoot straight up
            }
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
            int maxFrames = spriteComponent.SpriteData.Animations["MoveLeft"].Count;  // Total frames in the "MoveLeft" animation

            float normalizedSpeed = MathHelper.Clamp(Math.Abs(speedX) / MOVE_SPEED, 0, 1);
            int speedFrame = (int)(normalizedSpeed * (maxFrames - 1));
            spriteComponent.SetFrameIndex(speedFrame);

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
