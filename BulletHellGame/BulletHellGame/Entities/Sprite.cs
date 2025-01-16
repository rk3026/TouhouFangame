public class Sprite
{
    public Texture2D Texture { get; set; }
    public Vector2 Position { get; set; }
    public float Rotation { get; set; } = 0f;
    public Vector2 Scale { get; set; } = Vector2.One;

    public Sprite(Texture2D texture, Vector2 position)
    {
        Texture = texture;
        Position = position;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);
    }
}
