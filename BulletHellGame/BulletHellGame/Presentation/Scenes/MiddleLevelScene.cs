using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellGame.Presentation.Scenes;
// The user wants to use a 3x3 approach in the MidLevelDialogueScene.
// We'll create 3x3 sprite data for each character (Reimu, Marisa, Sakuya), referencing Pose1..Pose9.
// Then, in the draw call, we use, e.g. spriteData.Animations["Pose1"][0].
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public class MidLevelDialogueScene : IScene
{
    private ContentManager content;
    private GraphicsDevice graphics;
    private SpriteFont font;
    private Texture2D whitePixel;

    private Dictionary<string, SpriteData> characterPortraits = new();

    private string speaker;
    private string line;
    private string expressionKey = "Pose4";

    private float textAlpha = 0f;
    private float timer = 0f;
    private float displayDuration = 4f;

    private Vector2 shakeOffset = Vector2.Zero;
    private float shakeIntensity = 5f;
    private float shakeTimer = 0f;
    private float shakeDuration = 0.5f;

    public bool IsOverlay => true; // Keep it overlay so gameplay isn't paused
    public bool IsMenu { get; }

    public MidLevelDialogueScene(
        ContentManager content,
        GraphicsDevice graphics,
        string speakerName,
        string dialogueLine,
        string expression = "Pose4")
    {
        this.content = content;
        this.graphics = graphics;
        this.speaker = speakerName;
        this.line = dialogueLine;
        this.expressionKey = expression;
    }

    public void Load()
    {
        font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
        whitePixel = new Texture2D(graphics, 1, 1);
        whitePixel.SetData(new[] { Color.White });

        Texture2D reimuTex = content.Load<Texture2D>("SpriteSheets/CutsceneReimu");
        Texture2D marisaTex = content.Load<Texture2D>("SpriteSheets/CutsceneMarisa");
        Texture2D sakuyaTex = content.Load<Texture2D>("SpriteSheets/CutsceneSakuya");

        characterPortraits["Reimu Hakurei"] = TextureManager.Instance.Create3x3SpriteSheet(reimuTex, "CutsceneReimu");
        characterPortraits["Marisa Kirisame"] = TextureManager.Instance.Create3x3SpriteSheet(marisaTex, "CutsceneMarisa");
        characterPortraits["Sakuya Izayoi"] = TextureManager.Instance.Create3x3SpriteSheet(sakuyaTex, "CutsceneSakuya");
    }

    public void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        textAlpha = MathHelper.Clamp(textAlpha + 0.02f, 0f, 1f);
        timer += elapsed;

        if (shakeTimer < shakeDuration)
        {
            shakeTimer += elapsed;
            float offsetX = (float)(new Random().NextDouble() * 2 - 1) * shakeIntensity;
            float offsetY = (float)(new Random().NextDouble() * 2 - 1) * shakeIntensity;
            shakeOffset = new Vector2(offsetX, offsetY);
        }
        else
        {
            shakeOffset = Vector2.Zero;
        }

        if (timer >= displayDuration)
        {
            SceneManager.Instance.RemoveScene();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle screen = graphics.Viewport.Bounds;

        spriteBatch.End();
        spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(shakeOffset, 0)));

        spriteBatch.Draw(whitePixel, screen, Color.Black * 0.6f);

        Rectangle box = new Rectangle(0, screen.Height - 120, screen.Width, 120);
        spriteBatch.Draw(whitePixel, box, Color.Black * 0.7f);

        if (characterPortraits.TryGetValue(speaker, out var spriteData))
        {
            if (spriteData.Animations.TryGetValue(expressionKey, out var frames))
            {
                var rect = frames[0];
                float scale = 0.75f;
                Vector2 portraitPos = new Vector2(30, screen.Height - 120 - rect.Height * scale);

                spriteBatch.Draw(
                    spriteData.Texture,
                    portraitPos,
                    rect,
                    Color.White * textAlpha,
                    0f,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        spriteBatch.DrawString(font, speaker + ":", new Vector2(180, screen.Height - 110), Color.Cyan * textAlpha);
        spriteBatch.DrawString(font, line, new Vector2(180, screen.Height - 70), Color.White * textAlpha);

        spriteBatch.End();
        spriteBatch.Begin();
    }

    public class DialogueLine
    {
        public string Speaker { get; set; }
        public string Line { get; set; }
        public string Expression { get; set; }
    }
}