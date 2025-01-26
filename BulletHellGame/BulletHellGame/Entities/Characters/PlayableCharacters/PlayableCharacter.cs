using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities.Characters;
using BulletHellGame.Managers;
using System.Linq;

public class PlayableCharacter : Character
{
    public float MOVE_SPEED = 500f;
    private List<WeaponComponent> _weapons = new List<WeaponComponent>();

    public PlayableCharacter(SpriteData spriteData) : base(spriteData)
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
        this.UpdateAnimation();

        // Handle shooting input: if Space is pressed, call the Shoot method on the WeaponComponent
        if (InputManager.Instance.KeyDown(Keys.Space))
        {
            foreach (var weapon in _weapons)
            {
                weapon.Shoot(new List<Vector2> { Vector2.UnitY * -1 }); // Shoot straight up
            }
        }

        // Slow Mode:
        if (InputManager.Instance.KeyDown(Keys.LeftShift))
        {

        }

        base.Update(gameTime); // Ensure components are updated
    }

    private void UpdateAnimation()
    {
        MovementComponent movementComponent = this.GetComponent<MovementComponent>();
        float speedX = movementComponent.Velocity.X;

        SpriteComponent spriteComponent = this.GetComponent<SpriteComponent>();

        if (Math.Abs(speedX) > 0)
        {
            spriteComponent.SwitchAnimation("MoveLeft", false);
        }
        else  // When not moving horizontally
        {
            spriteComponent.SwitchAnimation("Idle", true);
        }
    }
}
