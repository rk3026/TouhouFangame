using System;
using System.Collections.Generic;
using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Presentation.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class Level1CutsceneScene : IScene
{
    public bool IsOverlay => false;

    private ContentManager content;
    private GraphicsDevice graphics;
    private Texture2D backgroundTexture;
    private Dictionary<string, SpriteData> characterPortraits = new();
    private SpriteFont font;
    private Texture2D whitePixel;

    private List<DialogueLine> dialogue;
    private int currentLine = 0;

    private float typeTimer = 0;
    private float typeDelay = 0.03f;
    private string visibleText = "";
    private bool textDone = false;

    private float flashTimer = 0f;
    private float flashDuration = 0.15f;
    private bool flashActive = false;

    public Level1CutsceneScene(ContentManager content, GraphicsDevice graphics)
    {
        this.content = content;
        this.graphics = graphics;
    }

    public void Load()
    {
        font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
        whitePixel = new Texture2D(graphics, 1, 1);
        whitePixel.SetData(new[] { Color.White });

        backgroundTexture = content.Load<Texture2D>("Backgrounds/touhou");
        characterPortraits["Reimu"] = TextureManager.Instance.GetSpriteData("ReimuSelect");
        characterPortraits["Marisa"] = TextureManager.Instance.GetSpriteData("MarisaSelect");
        characterPortraits["Sakuya"] = TextureManager.Instance.GetSpriteData("SakuyaSelect");

        BGMManager.Instance.PlayBGM(content, "02. Paradise ~ Deep Mountain");

        dialogue = new List<DialogueLine>
        {
            new DialogueLine { Speaker = "Reimu", Line = "Another quiet morning at the shrine...", Expression = "Default" },
            new DialogueLine { Speaker = "Marisa", Line = "Yo! Reimu, you up already?", Expression = "Happy" },
            new DialogueLine { Speaker = "Reimu", Line = "Of course I am. What's going on?", Expression = "Determined" },
            new DialogueLine { Speaker = "Sakuya", Line = "I sensed something unusual in the air today.", Expression = "Concerned" },
            new DialogueLine { Speaker = "Marisa", Line = "Guess it's another incident, huh?", Expression = "Default" }
        };
    }

    public void Update(GameTime gameTime)
    {
        var input = InputManager.Instance;
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (dialogue != null && currentLine < dialogue.Count)
        {
            if (!textDone)
            {
                typeTimer += elapsed;
                if (typeTimer > typeDelay && visibleText.Length < dialogue[currentLine].Line.Length)
                {
                    visibleText += dialogue[currentLine].Line[visibleText.Length];
                    typeTimer = 0f;
                }
                if (visibleText.Length == dialogue[currentLine].Line.Length)
                {
                    textDone = true;
                    flashActive = true;
                    flashTimer = 0f;
                }
            }
            else if (input.ActionPressed(GameAction.Select))
            {
                currentLine++;
                visibleText = "";
                textDone = false;
                flashActive = false;

                if (currentLine >= dialogue.Count)
                {
                    SceneManager.Instance.RemoveScene();
                    SceneManager.Instance.AddScene(new StageIntroScene(content, graphics));
                }
            }
        }

        if (flashActive)
        {
            flashTimer += elapsed;
            if (flashTimer > flashDuration)
            {
                flashActive = false;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        graphics.Clear(Color.Black);

        spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), Color.White);

        if (dialogue != null && currentLine < dialogue.Count)
        {
            DialogueLine line = dialogue[currentLine];

            if (characterPortraits.TryGetValue(line.Speaker, out var spriteData) && spriteData.Animations.TryGetValue(line.Expression, out var frames))
            {
                Rectangle sourceRect = frames[0];
                float scale = 1.0f;

                Vector2 position = new Vector2(
                    graphics.Viewport.Width / 2f - (sourceRect.Width * scale) / 2f,
                    graphics.Viewport.Height / 2f - (sourceRect.Height * scale) / 2f - 50
                );

                spriteBatch.Draw(
                    spriteData.Texture,
                    position,
                    sourceRect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0f);
            }

            Rectangle textbox = new Rectangle(50, graphics.Viewport.Height - 200, graphics.Viewport.Width - 100, 150);
            spriteBatch.Draw(whitePixel, textbox, Color.Black * 0.7f);
            spriteBatch.DrawString(font, line.Speaker + ":", new Vector2(textbox.X + 20, textbox.Y + 20), Color.Cyan);
            spriteBatch.DrawString(font, visibleText, new Vector2(textbox.X + 20, textbox.Y + 60), Color.White);
        }

        if (flashActive)
        {
            spriteBatch.Draw(whitePixel, new Rectangle(0, 0, graphics.Viewport.Width, graphics.Viewport.Height), Color.White * 0.25f);
        }
    }
}

public class StageIntroScene : IScene
{
    private ContentManager content;
    private GraphicsDevice graphics;
    private SpriteFont font;
    private float timer = 0f;
    private float displayDuration = 2.5f;

    public bool IsOverlay => false;

    public StageIntroScene(ContentManager content, GraphicsDevice graphics)
    {
        this.content = content;
        this.graphics = graphics;
    }

    public void Load()
    {
        font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
    }

    public void Update(GameTime gameTime)
    {
        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (timer >= displayDuration)
        {
            SceneManager.Instance.RemoveScene();
            SceneManager.Instance.AddScene(new TestLMScene(content, graphics, CharacterDataLoader.LoadCharacterData("Reimu")));
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        graphics.Clear(Color.Black);

        string text = "STAGE 1 START!";
        Vector2 textSize = font.MeasureString(text);
        Vector2 position = new Vector2(
            (graphics.Viewport.Width - textSize.X) / 2,
            (graphics.Viewport.Height - textSize.Y) / 2
        );

        spriteBatch.DrawString(font, text, position, Color.White);
    }
}
