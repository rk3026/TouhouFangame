using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.DataAccess.DataTransferObjects;
//using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.Presentation.Scenes;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Text;

namespace BulletHellGame.Presentation.Scenes
{
    public class CharacterSelectScene : IScene
    {
        private enum SelectionPhase { Difficulty, Heroine, Weapon }
        private SelectionPhase currentPhase = SelectionPhase.Difficulty;

        private string[] difficulties = { "Easy", "Normal", "Hard", "Lunatic" };
        private CharacterData[] characters;
        private SpriteFont _font;
        private SpriteData backgroundSprite;
        private SpriteData[] characterSprites;
        private int selectedIndex = 0;
        private int selectedDifficulty = 0;
        private int selectedHeroine = 0;
        private int selectedWeapon = 0;

        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;

        private bool tweeningDifficulty = false;
        private float difficultyTweenProgress = 0f;
        private const float tweenSpeed = 2f;
        private Vector2 difficultyStartPos;
        private Vector2 difficultyEndPos;

        public bool IsOverlay => false;

        public bool IsMenu => true;

        public CharacterSelectScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
        }

        public void Load()
        {
            _font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
            backgroundSprite = TextureManager.Instance.GetSpriteData("CharacterSelectBackground");
            characterSprites = new SpriteData[]
            {
                TextureManager.Instance.GetSpriteData("ReimuSelect"),
                TextureManager.Instance.GetSpriteData("MarisaSelect"),
                TextureManager.Instance.GetSpriteData("SakuyaSelect")
            };
            characters = new CharacterData[]
            {
                CharacterDataLoader.LoadCharacterData("Reimu"),
                EntityDataGenerator.CreateMarisaData(),
                EntityDataGenerator.CreateSakuyaData()
            };
            difficultyEndPos = new Vector2(_graphicsDevice.Viewport.X + 50, _graphicsDevice.Viewport.Y + _graphicsDevice.Viewport.Height - 50);
        }

        public void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentPhase > SelectionPhase.Difficulty && difficultyTweenProgress < 1f)
            {
                difficultyTweenProgress += tweenSpeed * delta;
                if (difficultyTweenProgress > 1f) difficultyTweenProgress = 1f;
            }
            else if (currentPhase == SelectionPhase.Difficulty && difficultyTweenProgress > 0f)
            {
                difficultyTweenProgress -= tweenSpeed * delta;
                if (difficultyTweenProgress < 0f) difficultyTweenProgress = 0f;
            }

            HandleMenuNavigation();

            if (currentPhase == SelectionPhase.Heroine)
            {
                selectedHeroine = selectedIndex;
            }
        }

        private void HandleMenuNavigation()
        {
            if (InputManager.Instance.ActionPressed(GameAction.MenuUp))
            {
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = GetCurrentOptions().Length - 1;
            }
            if (InputManager.Instance.ActionPressed(GameAction.MenuDown))
            {
                selectedIndex++;
                if (selectedIndex >= GetCurrentOptions().Length) selectedIndex = 0;
            }

            if (InputManager.Instance.ActionPressed(GameAction.Select))
            {
                switch (currentPhase)
                {
                    case SelectionPhase.Difficulty:
                        selectedDifficulty = selectedIndex;
                        difficultyStartPos = new Vector2(100, 100 + selectedDifficulty * 60);
                        break;
                    case SelectionPhase.Heroine:
                        selectedHeroine = selectedIndex;
                        break;
                    case SelectionPhase.Weapon:
                        var cutsceneData = CutsceneDataLoader.LoadCutsceneData("Level1Cutscenes");
 SceneManager.Instance.AddScene(new CutsceneScene(_contentManager, _graphicsDevice, cutsceneData, characters[selectedHeroine]));
                        return;
                }
                selectedIndex = 0;
                currentPhase++;
            }

            if (InputManager.Instance.ActionPressed(GameAction.Pause))
            {
                if (currentPhase > SelectionPhase.Difficulty)
                {
                    currentPhase--;
                    selectedIndex = 0;
                }
                else
                {
                    SceneManager.Instance.RemoveScene();
                }
            }
        }

        private string[] GetCurrentOptions()
        {
            return currentPhase switch
            {
                SelectionPhase.Difficulty => difficulties,
                SelectionPhase.Heroine => characters.Select(c => c.Name).ToArray(),
                SelectionPhase.Weapon => characters[selectedHeroine].ShotTypes.Select(s => s.Name).ToArray(),
                _ => new string[0],
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundSprite.Texture, Vector2.Zero, backgroundSprite.Animations["Default"][0], Color.White);

            string phaseText = currentPhase switch
            {
                SelectionPhase.Difficulty => "Select a Difficulty",
                SelectionPhase.Heroine => "Select a Heroine",
                SelectionPhase.Weapon => "Select a Weapon",
                _ => ""
            };

            Vector2 titleSize = _font.MeasureString(phaseText);
            Vector2 titlePosition = new Vector2((_graphicsDevice.Viewport.Width - titleSize.X) / 2, 20);
            spriteBatch.DrawString(_font, phaseText, titlePosition, Color.White);

            // Draw difficulty at bottom left if selected
            if (currentPhase > SelectionPhase.Difficulty || difficultyTweenProgress > 0f)
            {
                Vector2 difficultyPosition = Vector2.Lerp(difficultyStartPos, difficultyEndPos, difficultyTweenProgress);
                spriteBatch.DrawString(_font, difficulties[selectedDifficulty], difficultyPosition, Color.White);
            }

            // Draw selected heroine portrait
            if (currentPhase >= SelectionPhase.Heroine && selectedHeroine >= 0 && characterSprites[selectedHeroine] != null)
            {
                float opacity = currentPhase > SelectionPhase.Heroine ? 0.3f : 1f;
                Vector2 portraitPosition = new Vector2(300, 100);
                spriteBatch.Draw(characterSprites[selectedHeroine].Texture, portraitPosition, characterSprites[selectedHeroine].Animations["Default"][0], Color.White * opacity);
            }

            // Draw current options
            float descriptionScale = 0.7f;
            float maxDescriptionWidth = _graphicsDevice.Viewport.Width - 50; // Adjust as needed
            string[] options = GetCurrentOptions();
            float yOffset = 100; // Initial Y offset

            for (int i = 0; i < options.Length; i++)
            {
                Vector2 optionPosition = new Vector2(100, yOffset);
                Color textColor = (i == selectedIndex) ? Color.Red : Color.White;

                // Draw the option
                spriteBatch.DrawString(_font, options[i], optionPosition, textColor);

                if (currentPhase == SelectionPhase.Weapon)
                {
                    yOffset += _font.LineSpacing; // Move down after option
                    var shotType = characters[selectedHeroine].ShotTypes[i];
                    string focusedDescription = shotType.FocusedShot?.Description ?? "No description available";
                    string unfocusedDescription = shotType.UnfocusedShot?.Description ?? "No description available";

                    string wrappedFocusedDescription = WrapText(_font, focusedDescription, maxDescriptionWidth);
                    string wrappedUnfocusedDescription = WrapText(_font, unfocusedDescription, maxDescriptionWidth);

                    // Draw "Focused Shot" label at normal size
                    Vector2 focusedLabelPos = new Vector2(120, yOffset);
                    spriteBatch.DrawString(_font, "Focused Shot:", focusedLabelPos, Color.LightGray);
                    yOffset += _font.LineSpacing; // Move down after label

                    // Draw wrapped description in smaller size
                    Vector2 focusedShotPos = new Vector2(140, yOffset);
                    spriteBatch.DrawString(_font, wrappedFocusedDescription, focusedShotPos, Color.LightGray, 0f, Vector2.Zero, descriptionScale, SpriteEffects.None, 0f);
                    float wrappedFocusedHeight = _font.MeasureString(wrappedFocusedDescription).Y * descriptionScale;
                    yOffset += wrappedFocusedHeight;

                    // Draw "Unfocused Shot" label at normal size
                    Vector2 unfocusedLabelPos = new Vector2(120, yOffset + 10); // Small gap
                    spriteBatch.DrawString(_font, "Unfocused Shot:", unfocusedLabelPos, Color.LightGray);
                    yOffset += _font.LineSpacing + 10; // Move down after label

                    // Draw wrapped description in smaller size
                    Vector2 unfocusedShotPos = new Vector2(140, yOffset);
                    spriteBatch.DrawString(_font, wrappedUnfocusedDescription, unfocusedShotPos, Color.LightGray, 0f, Vector2.Zero, descriptionScale, SpriteEffects.None, 0f);
                    float wrappedUnfocusedHeight = _font.MeasureString(wrappedUnfocusedDescription).Y * descriptionScale;
                }
                yOffset += 60; // Space between options
            }
        }



        private string WrapText(SpriteFont font, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder wrappedText = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;

            foreach (var word in words)
            {
                Vector2 wordSize = font.MeasureString(word);

                // Check if adding the word would exceed the max width
                if (lineWidth + wordSize.X > maxLineWidth)
                {
                    wrappedText.AppendLine();
                    lineWidth = 0f;
                }

                wrappedText.Append(word + " ");
                lineWidth += wordSize.X + spaceWidth;
            }

            return wrappedText.ToString();
        }

    }
}
