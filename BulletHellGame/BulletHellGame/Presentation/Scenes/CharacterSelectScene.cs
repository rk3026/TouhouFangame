using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.Presentation.Scenes
{
    public class CharacterSelectScene : IScene
    {
        private CharacterData[] characters;
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
            characters = new CharacterData[]
            {
                CharacterDataLoader.LoadCharacterData("Reimu"),
                CharacterDataLoader.LoadCharacterData("Marisa"),
                CharacterDataLoader.LoadCharacterData("Sakuya")
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
                SceneManager.Instance.AddScene(new TestScene(_contentManager, _graphicsDevice, characters[selectedIndex]));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                Vector2 position = new Vector2(100, 100 + i * 60);
                Color textColor = (i == selectedIndex) ? Color.Red : Color.White;
                string text = characters[i].Name;

                if (i == selectedIndex)
                {
                    float time = (float)(_gameTime?.TotalGameTime.TotalSeconds ?? 0f);
                    float offsetX = (float)Math.Sin(time * 5) * 3;
                    float offsetY = (float)Math.Cos(time * 5) * 3;
                    Vector2 animatedPosition = position + new Vector2(offsetX, offsetY);
                    spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), text, animatedPosition, textColor);
                }
                else
                {
                    spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), text, position, textColor);
                }
            }
        }
    }
}
