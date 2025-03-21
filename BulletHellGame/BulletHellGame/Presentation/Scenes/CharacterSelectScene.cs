using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.Presentation.Scenes
{
    public class CharacterSelectScene : IScene
    {
        private CharacterData[] characters;
        private SpriteData backgroundSprite;
        private SpriteData[] characterSprites;
        private int selectedIndex = 0;
        private GameTime _gameTime;
        private ContentManager _contentManager;
        private GraphicsDevice _graphicsDevice;

        public CharacterSelectScene(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            _contentManager = contentManager;
            _graphicsDevice = graphicsDevice;
        }

        public void Load()
        {
            // Load sprite data
            backgroundSprite = TextureManager.Instance.GetSpriteData("CharacterSelectBackground");
            characterSprites = new SpriteData[]
            {
                TextureManager.Instance.GetSpriteData("ReimuSelect"),
                TextureManager.Instance.GetSpriteData("MarisaSelect"),
                TextureManager.Instance.GetSpriteData("SakuyaSelect")
            };

            // Load character data
            characters = new CharacterData[]
            {
                CharacterDataLoader.LoadCharacterData("Reimu"),
                EntityDataGenerator.CreateMarisaData(), //Eventually, replace with: CharacterDataLoader.LoadCharacterData("Marisa"),
                EntityDataGenerator.CreateSakuyaData(), //Eventually, replace with: CharacterDataLoader.LoadCharacterData("Sakuya")
            };
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            if (InputManager.Instance.ActionPressed(GameAction.MenuUp))
            {
                selectedIndex--;
                if (selectedIndex < 0) selectedIndex = characters.Length - 1;
            }
            if (InputManager.Instance.ActionPressed(GameAction.MenuDown))
            {
                selectedIndex++;
                if (selectedIndex >= characters.Length) selectedIndex = 0;
            }
            if (InputManager.Instance.ActionPressed(GameAction.Select))
            {
                SceneManager.Instance.AddScene(new TestLMScene(_contentManager, _graphicsDevice, characters[selectedIndex]));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            spriteBatch.Draw(backgroundSprite.Texture, Vector2.Zero, backgroundSprite.Animations["Default"][0], Color.White);

            // Draw character names
            for (int i = 0; i < characters.Length; i++)
            {
                Vector2 namePosition = new Vector2(100, 100 + i * 60);
                Color textColor = (i == selectedIndex) ? Color.Red : Color.White;
                spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), characters[i].Name, namePosition, textColor);
            }

            // Draw selected character portrait
            if (characterSprites[selectedIndex] != null)
            {
                Vector2 portraitPosition = new Vector2(300, 100);
                spriteBatch.Draw(characterSprites[selectedIndex].Texture, portraitPosition, characterSprites[selectedIndex].Animations["Default"][0], Color.White);
            }
        }
    }
}