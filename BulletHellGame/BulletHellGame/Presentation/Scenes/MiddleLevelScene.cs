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
    public bool IsOverlay => true;

    private ContentManager content;
    private GraphicsDevice graphics;
    private SpriteFont font;
    private Texture2D whitePixel;

    // dictionary of speaker => 3x3 SpriteData
    private Dictionary<string, SpriteData> characterPortraits = new();

    private string speaker;
    private string line;
    private string expressionKey = "Pose3"; // default to Pose1 in 3x3 sprite sheet

    private float textAlpha = 0f;
    private float timer = 0f;
    private float displayDuration = 4f;

    public MidLevelDialogueScene(
        ContentManager content,
        GraphicsDevice graphics,
        string speakerName,
        string dialogueLine,
        string expression = "Pose3")
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

        // Reuse the EXACT SAME PATHS and method as in Level1Cutscene.
        // So they remain consistent:
        Texture2D reimuTex = content.Load<Texture2D>("SpriteSheets/CutsceneReimu");
        Texture2D marisaTex = content.Load<Texture2D>("SpriteSheets/CutsceneMarisa");
        Texture2D sakuyaTex = content.Load<Texture2D>("SpriteSheets/CutsceneSakuya");

        // Slice them 3x3 with the same keys (Pose1..Pose9)
        var reimuData = TextureManager.Instance.Create3x3SpriteSheet(reimuTex, "ReimuCutscene");
        var marisaData = TextureManager.Instance.Create3x3SpriteSheet(marisaTex, "MarisaCutscene");
        var sakuyaData = TextureManager.Instance.Create3x3SpriteSheet(sakuyaTex, "SakuyaCutscene");

        // map speaker names to these data objects
        characterPortraits["Reimu Hakurei"] = reimuData;
        characterPortraits["Marisa Kirisame"] = marisaData;
        characterPortraits["Sakuya Izayoi"] = sakuyaData;
    }

    public void Update(GameTime gameTime)
    {
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        // fade in text
        textAlpha = MathHelper.Clamp(textAlpha + 0.02f, 0f, 1f);

        // remove after duration
        timer += elapsed;
        if (timer >= displayDuration)
        {
            SceneManager.Instance.RemoveScene();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle screen = graphics.Viewport.Bounds;

        // dim the bottom area for text
        Rectangle box = new Rectangle(0, screen.Height - 120, screen.Width, 120);
        spriteBatch.Draw(whitePixel, box, Color.Black * 0.6f);

        // draw speaker portrait if found
        if (characterPortraits.TryGetValue(speaker, out var spriteData))
        {
            if (spriteData.Animations.TryGetValue(expressionKey, out var frames))
            {
                var rect = frames[0];

                // scale down the portrait
                float portraitScale = .76f;
                float scaledHeight = rect.Height * portraitScale;

                // position near left side above text strip
                Vector2 portraitPos = new Vector2(
                    30, // left offset
                    screen.Height - 120 - scaledHeight // above the text box
                );

                // Draw with scale
                spriteBatch.Draw(
                    spriteData.Texture,
                    portraitPos,
                    rect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    portraitScale,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        // draw text
        spriteBatch.DrawString(font, speaker + ":", new Vector2(180, screen.Height - 110), Color.Cyan * textAlpha);
        spriteBatch.DrawString(font, line, new Vector2(180, screen.Height - 70), Color.White * textAlpha);
    }

    // Example DialogueLine for reference
    public class DialogueLine
    {
        public string Speaker { get; set; }
        public string Line { get; set; }
        public string Expression { get; set; }
    }
}
