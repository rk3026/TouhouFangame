namespace BulletHellGame.Managers
{
    using BulletHellGame.Scenes;
    using Microsoft.Xna.Framework.Content;

    public class GameManager
    {
        private readonly SceneManager _sceneManager;
        private readonly TextureManager _textureManager;

        public GameManager()
        {
            _textureManager = TextureManager.Instance;
            _sceneManager = SceneManager.Instance;
        }

        public void Update()
        {
            if (_sceneManager.Ready)
            {
                var newScene = _sceneManager.ActiveScene == Scenes.MainMenu ? Scenes.GameplayScene : Scenes.MainMenu;

                if (InputManager.KeyPressed(Keys.F1)) _sceneManager.SwitchScene(newScene, Transitions.Fade);
                else if (InputManager.KeyPressed(Keys.F2)) _sceneManager.SwitchScene(newScene, Transitions.Wipe);
                else if (InputManager.KeyPressed(Keys.F3)) _sceneManager.SwitchScene(newScene, Transitions.Push);
                else if (InputManager.KeyPressed(Keys.F4)) _sceneManager.SwitchScene(newScene, Transitions.Curtains);
                else if (InputManager.KeyPressed(Keys.F5)) _sceneManager.SwitchScene(newScene, Transitions.Rectangle);
                else if (InputManager.KeyPressed(Keys.F6)) _sceneManager.SwitchScene(newScene, Transitions.Checker);
            }

            _sceneManager.Update();
        }

        public void Draw()
        {
            var frame = _sceneManager.GetFrame();

            Globals.SpriteBatch.Begin();
            Globals.SpriteBatch.Draw(frame, Vector2.Zero, Color.White);
            Globals.SpriteBatch.End();
        }
    }
}