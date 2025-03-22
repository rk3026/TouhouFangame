using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace BulletHellGame.Presentation.Scenes
{
    public class CharacterSelectScene : IScene
    {
        private enum SelectionPhase { Difficulty, Heroine, Weapon }
        private SelectionPhase currentPhase = SelectionPhase.Difficulty;

        private string[] difficulties = { "Easy", "Normal", "Hard", "Lunatic" }; // Placeholder, pass the difficulty when loading the gameplayscene...
        private CharacterData[] characters;
        private string[] weapons = { "Weapon A", "Weapon B" }; // Placeholder

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
        private Vector2 difficultyEndPos = new Vector2(50, 380);

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
                        selectedWeapon = selectedIndex;
                        SceneManager.Instance.AddScene(new TestScene(_contentManager, _graphicsDevice, characters[selectedHeroine]));
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

            if (currentPhase == SelectionPhase.Heroine)
            {
                selectedHeroine = selectedIndex;
            }
        }

        private string[] GetCurrentOptions()
        {
            return currentPhase switch
            {
                SelectionPhase.Difficulty => difficulties,
                SelectionPhase.Heroine => characters.Select(c => c.Name).ToArray(),
                SelectionPhase.Weapon => weapons,
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

            // Draw current options
            string[] options = GetCurrentOptions();
            for (int i = 0; i < options.Length; i++)
            {
                Vector2 optionPosition = new Vector2(100, 100 + i * 60);
                Color textColor = (i == selectedIndex) ? Color.Red : Color.White;
                spriteBatch.DrawString(_font, options[i], optionPosition, textColor);
            }

            // Draw selected heroine portrait
            if (currentPhase >= SelectionPhase.Heroine && selectedHeroine >= 0 && characterSprites[selectedHeroine] != null)
            {
                float opacity = currentPhase > SelectionPhase.Heroine ? 0.5f : 1f;
                Vector2 portraitPosition = new Vector2(300, 100);
                spriteBatch.Draw(characterSprites[selectedHeroine].Texture, portraitPosition, characterSprites[selectedHeroine].Animations["Default"][0], Color.White*opacity);
            }
        }
    }
}