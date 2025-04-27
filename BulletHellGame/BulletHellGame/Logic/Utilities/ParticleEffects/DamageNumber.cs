using BulletHellGame.Logic.Utilities.ParticleEffects;

public class DamageNumber : ParticleEffect
{
    private int _damage;
    private SpriteFont _font;
    private Color _color;
    private float _alpha;
    private float _rotation;
    private float _scale;

    public DamageNumber(Vector2 position, int damage, SpriteFont font, Color color, float lifeTime, float rotation, float scale)
        : base(position, new List<Texture2D>(), lifeTime, 0)
    {
        _damage = damage;
        _font = font;
        _color = color;
        _alpha = 1f;
        _rotation = rotation;
        _scale = scale;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Make it float up with slight drift
        Position += new Vector2(0, -30f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        Position += new Vector2((float)Math.Sin(LifeTime * 5) * 2, 0); // Slight horizontal oscillation

        // Fade out effect
        _alpha = Math.Max(0, LifeTime / 1f);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (IsComplete) return;

        spriteBatch.DrawString(_font, _damage.ToString(), Position, _color * _alpha, _rotation,
            _font.MeasureString(_damage.ToString()) / 2, _scale, SpriteEffects.None, 0f);
    }
}
