using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Presentation.Scenes;
using Microsoft.Xna.Framework.Content;

public class WinScene : IScene
{
    private Rectangle _menuLocation;
    private ContentManager _content;
    private GraphicsDevice _graphics;
    private SpriteFont _font;
    private Texture2D _whitePixel;
    private CharacterData _characterData;
    private EntityManager _entityManager;
    private GameUI _gameUI;

    private SpriteData _background;
    private Dictionary<string, SpriteData> characterPortraits = new();
    private string dialogueLine = "Good job! We did it!";

    private float textAlpha = 0f;
    private float bounceTimer = 0f;
    private const float bounceSpeed = 2.5f;

    public bool IsOverlay => false;

    public WinScene(Rectangle menuLocation, ContentManager content, GraphicsDevice graphics, CharacterData characterData, EntityManager entityManager)
    {
        _menuLocation = menuLocation;
        _content = content;
        _graphics = graphics;
        _characterData = characterData;
        _entityManager = entityManager;
    }

    public void Load()
    {
        _font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
        _whitePixel = new Texture2D(_graphics, 1, 1);
        _whitePixel.SetData(new[] { Color.White });

        var reimu = _content.Load<Texture2D>("SpriteSheets/CutsceneReimu");
        var marisa = _content.Load<Texture2D>("SpriteSheets/CutsceneMarisa");
        var sakuya = _content.Load<Texture2D>("SpriteSheets/CutsceneSakuya");

        characterPortraits["Reimu Hakurei"] = TextureManager.Instance.Create3x3SpriteSheet(reimu, "ReimuCutscene");
        characterPortraits["Marisa Kirisame"] = TextureManager.Instance.Create3x3SpriteSheet(marisa, "MarisaCutscene");
        characterPortraits["Sakuya Izayoi"] = TextureManager.Instance.Create3x3SpriteSheet(sakuya, "SakuyaCutscene");

        _gameUI = new GameUI(_font, _menuLocation, _entityManager);
        _background = TextureManager.Instance.GetSpriteData("MainMenu");

        BGMManager.Instance.PlayBGM(_content, "02. Paradise ~ Deep Mountain");
    }

    public void Update(GameTime gameTime)
    {
        float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        textAlpha = MathHelper.Clamp(textAlpha + delta * 0.6f, 0f, 1f);
        bounceTimer += delta;

        _gameUI.Update(gameTime);

        if (InputManager.Instance.ActionPressed(GameAction.Select))
        {
            SceneManager.Instance.RemoveScene();
            SceneManager.Instance.AddScene(new TestLMScene(_content, _graphics, _characterData));
        }

        if (InputManager.Instance.ActionPressed(GameAction.Pause))
        {
            ;
            SceneManager.Instance.AddScene(new MainMenuScene(_content, _graphics));
        }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        // Background
        spriteBatch.Draw(_background.Texture, _graphics.Viewport.Bounds, _background.Animations["Default"][0], Color.White * 0.5f);

        int splitMidX = _menuLocation.X + _menuLocation.Width / 2;

        // Character Portrait (bottom-left)
        if (characterPortraits.TryGetValue(_characterData.Name, out var spriteData))
        {
            if (spriteData.Animations.TryGetValue("Pose1", out var frames))
            {
                var rect = frames[0];
                float scale = 0.51F;

                Vector2 pos = new Vector2(
                    _menuLocation.X + -50,
                    _menuLocation.Y + _menuLocation.Height - rect.Height * scale - 20
                );

                spriteBatch.Draw(spriteData.Texture, pos, rect, Color.White * textAlpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }

        // Title and message
        string title = "Victory!";
        string msg = _characterData.Name + ": " + dialogueLine;

        Vector2 titlePos = new Vector2(splitMidX + 20, _menuLocation.Y + 30);
        Vector2 msgPos = new Vector2(splitMidX + 20, _menuLocation.Y + 80);
        spriteBatch.DrawString(_font, title, titlePos, Color.Gold * textAlpha);
        spriteBatch.DrawString(_font, msg, msgPos, Color.White * textAlpha);

        // Stats
        Rectangle statsArea = new Rectangle(splitMidX + 20, (int)(msgPos.Y + 40), _menuLocation.Width / 2 - 40, _menuLocation.Height - 100);
        GameUI statsUI = new GameUI(_font, statsArea, _entityManager);
        statsUI.Draw(spriteBatch);

        // Bouncy fade-in text
        float bounceOffset = (float)Math.Sin(bounceTimer * 4f) * 5f;

        Vector2 playAgainPos = new Vector2(splitMidX + 20, statsArea.Bottom - 40 + bounceOffset);
        Vector2 mainMenuPos = new Vector2(splitMidX + 20, statsArea.Bottom - 10 + bounceOffset);

        spriteBatch.DrawString(_font, "Press Enter to Play Again", playAgainPos, Color.LimeGreen * textAlpha);
        spriteBatch.DrawString(_font, "Press ESC to Exit to Main Menu", mainMenuPos, Color.LightBlue * textAlpha);
    }
}


