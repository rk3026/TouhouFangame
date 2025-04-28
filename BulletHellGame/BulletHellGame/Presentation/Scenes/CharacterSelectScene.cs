using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using BulletHellGame.Presentation.UI.Menu;
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
        private SpriteData backgroundSprite;
        private SpriteData[] characterSprites;
        private SpriteFont _font;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;

        private List<MenuOption> menuOptions;
        private MenuNavigator menuNavigator;

        private int selectedDifficulty = 0;
        private int selectedHeroine = 0;
        private int selectedWeapon = 0;

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
                TextureManager.Instance.GetSpriteData("SakuyaSelect"),
                TextureManager.Instance.GetSpriteData("ReimuSelect"),
            };
            characters = new CharacterData[]
            {
                CharacterDataLoader.LoadCharacterData("Reimu"),
                EntityDataGenerator.CreateMarisaData(),
                EntityDataGenerator.CreateSakuyaData(),
                EntityDataGenerator.CreateEpicTestData()
            };

            difficultyEndPos = new Vector2(_graphicsDevice.Viewport.X + 50, _graphicsDevice.Viewport.Y + _graphicsDevice.Viewport.Height - 50);

            BuildMenuOptions();
        }

        private void BuildMenuOptions()
        {
            var style = new MenuOptionStyle1(_contentManager, _graphicsDevice);
            menuOptions = GetCurrentOptions().Select(option => new MenuOption(option, () => { }, style)).ToList();
            menuNavigator = new MenuNavigator(menuOptions.Count);
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

            menuNavigator.Update(gameTime);

            if (InputManager.Instance.ActionPressed(GameAction.Select))
            {
                HandleSelection();
            }

            if (InputManager.Instance.ActionPressed(GameAction.Pause))
            {
                if (currentPhase > SelectionPhase.Difficulty)
                {
                    currentPhase--;
                    BuildMenuOptions();
                }
                else
                {
                    SceneManager.Instance.RemoveScene();
                }
            }
        }

        private void HandleSelection()
        {
            int selected = menuNavigator.SelectedIndex;

            switch (currentPhase)
            {
                case SelectionPhase.Difficulty:
                    selectedDifficulty = selected;
                    difficultyStartPos = new Vector2(100, 100 + selectedDifficulty * 60);
                    break;
                case SelectionPhase.Heroine:
                    selectedHeroine = selected;
                    break;
                case SelectionPhase.Weapon:
                    selectedWeapon = selected;
                    SceneManager.Instance.AddScene(new TestLMScene(_contentManager, _graphicsDevice, characters[selectedHeroine]));
                    return;
            }

            currentPhase++;
            BuildMenuOptions();
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

            if (currentPhase > SelectionPhase.Difficulty || difficultyTweenProgress > 0f)
            {
                Vector2 difficultyPosition = Vector2.Lerp(difficultyStartPos, difficultyEndPos, difficultyTweenProgress);
                spriteBatch.DrawString(_font, difficulties[selectedDifficulty], difficultyPosition, Color.White);
            }

            if (currentPhase >= SelectionPhase.Heroine && selectedHeroine >= 0 && characterSprites[selectedHeroine] != null)
            {
                float opacity = currentPhase > SelectionPhase.Heroine ? 0.3f : 1f;
                Vector2 portraitPosition = new Vector2(300, 100);
                spriteBatch.Draw(characterSprites[selectedHeroine].Texture, portraitPosition, characterSprites[selectedHeroine].Animations["Default"][0], Color.White * opacity);
            }

            float yOffset = 100;
            float descriptionScale = 0.7f;
            float maxDescriptionWidth = _graphicsDevice.Viewport.Width - 50;

            for (int i = 0; i < menuOptions.Count; i++)
            {
                Vector2 position = new Vector2(100, yOffset);
                menuOptions[i].Draw(spriteBatch, null, i, position, menuNavigator.SelectedIndex);

                if (currentPhase == SelectionPhase.Weapon)
                {
                    yOffset += _font.LineSpacing;
                    var shotType = characters[selectedHeroine].ShotTypes[i];
                    string focusedDesc = shotType.FocusedShot?.Description ?? "No description available";
                    string unfocusedDesc = shotType.UnfocusedShot?.Description ?? "No description available";

                    Vector2 focusedLabelPos = new Vector2(120, yOffset);
                    spriteBatch.DrawString(_font, "Focused Shot:", focusedLabelPos, Color.LightGray);
                    yOffset += _font.LineSpacing;

                    Vector2 focusedDescPos = new Vector2(140, yOffset);
                    spriteBatch.DrawString(_font, WrapText(_font, focusedDesc, maxDescriptionWidth), focusedDescPos, Color.LightGray, 0f, Vector2.Zero, descriptionScale, SpriteEffects.None, 0f);
                    yOffset += _font.MeasureString(focusedDesc).Y * descriptionScale;

                    Vector2 unfocusedLabelPos = new Vector2(120, yOffset + 10);
                    spriteBatch.DrawString(_font, "Unfocused Shot:", unfocusedLabelPos, Color.LightGray);
                    yOffset += _font.LineSpacing + 10;

                    Vector2 unfocusedDescPos = new Vector2(140, yOffset);
                    spriteBatch.DrawString(_font, WrapText(_font, unfocusedDesc, maxDescriptionWidth), unfocusedDescPos, Color.LightGray, 0f, Vector2.Zero, descriptionScale, SpriteEffects.None, 0f);
                }

                yOffset += 60;
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
                Vector2 size = font.MeasureString(word);
                if (lineWidth + size.X > maxLineWidth)
                {
                    wrappedText.AppendLine();
                    lineWidth = 0f;
                }
                wrappedText.Append(word + " ");
                lineWidth += size.X + spaceWidth;
            }
            return wrappedText.ToString();
        }
    }
}
