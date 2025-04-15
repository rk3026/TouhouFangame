using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Text;

namespace BulletHellGame.Presentation.Scenes
{
    public class MultiCharacterSelectScene : IScene
    {
        private enum SelectionPhase { Difficulty, Heroine, Weapon }
        private SelectionPhase currentPhase = SelectionPhase.Difficulty;

        private string[] difficulties = { "Easy", "Normal", "Hard", "Lunatic" };
        private CharacterData[] characters;
        private SpriteFont _font;
        private SpriteData backgroundSprite;
        private SpriteData[] characterSprites;

        private int[] selectedIndex = new int[2];
        private int[] selectedDifficulty = new int[2];
        private int[] selectedHeroine = new int[2];
        private int[] selectedWeapon = new int[2];

        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;

        public bool IsOverlay => false;

        public MultiCharacterSelectScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
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
            for (int player = 0; player < 2; player++)
            {
                GameAction upAction = (player == 0) ? GameAction.MenuUp : GameAction.MenuUpP2;
                GameAction downAction = (player == 0) ? GameAction.MenuDown : GameAction.MenuDownP2;
                GameAction selectAction = (player == 0) ? GameAction.Select : GameAction.SelectP2;
                GameAction backAction = (player == 0) ? GameAction.Pause : GameAction.PauseP2;

                if (InputManager.Instance.ActionPressed(upAction))
                {
                    selectedIndex[player]--;
                    if (selectedIndex[player] < 0) selectedIndex[player] = GetCurrentOptions().Length - 1;
                }
                if (InputManager.Instance.ActionPressed(downAction))
                {
                    selectedIndex[player]++;
                    if (selectedIndex[player] >= GetCurrentOptions().Length) selectedIndex[player] = 0;
                }

                if (InputManager.Instance.ActionPressed(selectAction))
                {
                    switch (currentPhase)
                    {
                        case SelectionPhase.Difficulty:
                            selectedDifficulty[player] = selectedIndex[player];
                            break;
                        case SelectionPhase.Heroine:
                            selectedHeroine[player] = selectedIndex[player];
                            break;
                        case SelectionPhase.Weapon:
                            selectedWeapon[player] = selectedIndex[player];
                            if (player == 1)
                            {
                                SceneManager.Instance.AddScene(new Level1CutsceneScene(_contentManager, _graphicsDevice, characters[selectedHeroine[0]], characters[selectedHeroine[1]]));
                                return;
                            }
                            break;
                    }
                }
            }
        }

        private string[] GetCurrentOptions()
        {
            return currentPhase switch
            {
                SelectionPhase.Difficulty => difficulties,
                SelectionPhase.Heroine => characters.Select(c => c.Name).ToArray(),
                SelectionPhase.Weapon => characters[selectedHeroine[0]].ShotTypes.Select(s => s.Name).ToArray(),
                _ => new string[0],
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundSprite.Texture, Vector2.Zero, backgroundSprite.Animations["Default"][0], Color.White);

            for (int player = 0; player < 2; player++)
            {
                string phaseText = currentPhase switch
                {
                    SelectionPhase.Difficulty => $"Player {player + 1}: Select a Difficulty",
                    SelectionPhase.Heroine => $"Player {player + 1}: Select a Heroine",
                    SelectionPhase.Weapon => $"Player {player + 1}: Select a Weapon",
                    _ => ""
                };

                Vector2 titlePosition = new Vector2((_graphicsDevice.Viewport.Width / 2) * player + 50, 20);
                spriteBatch.DrawString(_font, phaseText, titlePosition, Color.White);

                string[] options = GetCurrentOptions();
                float yOffset = 100;
                for (int i = 0; i < options.Length; i++)
                {
                    Vector2 optionPosition = new Vector2((_graphicsDevice.Viewport.Width / 2) * player + 100, yOffset);
                    Color textColor = (i == selectedIndex[player]) ? Color.Red : Color.White;
                    spriteBatch.DrawString(_font, options[i], optionPosition, textColor);
                    yOffset += 60;
                }
            }
        }
    }
}
