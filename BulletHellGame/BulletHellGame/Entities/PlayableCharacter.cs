using BulletHellGame.Components;
using BulletHellGame.Entities;

public class PlayableCharacter : Entity
{
    public PlayableCharacter(Texture2D texture, Vector2 position) : base(texture, position)
    {
        AddComponent(new HealthComponent(100));         // Health handling
        AddComponent(new MovementComponent(this, new Vector2(0,0))); // Movement handling
        AddComponent(new WeaponComponent(this));        // Shooting logic
        //this.GetComponent<MovementComponent>().Velocity = Vector2.Zero;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime); // Ensure components are updated
    }
}
