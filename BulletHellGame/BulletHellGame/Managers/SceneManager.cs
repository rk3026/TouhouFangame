using BulletHellGame.Scenes;
using System.Linq;

namespace BulletHellGame.Managers
{
    public class SceneManager
    {
        private static SceneManager _instance;
        public static SceneManager Instance => _instance ??= new SceneManager();

        private readonly Stack<IScene> _sceneStack = new Stack<IScene>();
        private readonly Dictionary<Transitions, Transition> _transitions = new Dictionary<Transitions, Transition>();

        private IScene _newScene;

        public IReadOnlyCollection<IScene> SceneStack => _sceneStack;

        private SceneManager()
        {
            // Initialize transitions:
            _transitions.Add(Transitions.Fade, new FadeTransition());
            _transitions.Add(Transitions.Wipe, new WipeTransition());
            _transitions.Add(Transitions.Push, new PushTransition());
            _transitions.Add(Transitions.Curtains, new CurtainsTransition());
            _transitions.Add(Transitions.Rectangle, new RectangleTransition());
            _transitions.Add(Transitions.Checker, new CheckerTransition());
        }

        public void AddScene(IScene newScene)
        {
            newScene.Load();
            _sceneStack.Push(newScene);
        }

        public void RemoveScene()
        {
            if (_sceneStack.Count == 0) return;
            _sceneStack.Pop();
        }

        public void ClearScenes()
        {
            _sceneStack.Clear();
        }

        public IScene GetCurrentScene()
        {
            return _sceneStack.Count > 0 ? _sceneStack.Peek() : null;
        }

        public void Update(GameTime gameTime)
        {
            // Update the current scene
            _sceneStack.Peek()?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            // If the top is a pause screen, also draw the scene below it
            if (_sceneStack.Count > 1 && _sceneStack.Peek() is PausedScene || _sceneStack.Peek() is RetryMenuScene)
            {
                _sceneStack.ElementAt(_sceneStack.Count - 2)?.Draw(spriteBatch);
            }
            _sceneStack.Peek()?.Draw(spriteBatch);

            if (SettingsManager.Instance.Debugging) DrawSceneStackDebug(spriteBatch);
        }

        private void DrawSceneStackDebug(SpriteBatch spriteBatch)
        {
            // Debugging: Draw the scene stack count and the scene names
            string debugText = $"Scene Stack Count: {SceneManager.Instance.SceneStack.Count}";
            Vector2 debugPosition = new Vector2(10, 10);
            Vector2 outlineOffset = new Vector2(3, 3);
            spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), debugText, debugPosition + outlineOffset, Color.Black);
            spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), debugText, debugPosition, Color.White);
            debugPosition.Y += 20;
            foreach (var scene in SceneStack)
            {
                string sceneName = scene.GetType().Name;
                spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), sceneName, debugPosition + outlineOffset, Color.Black);
                spriteBatch.DrawString(FontManager.Instance.GetFont("DFPPOPCorn-W12"), sceneName, debugPosition, Color.White);
                debugPosition.Y += 20;
            }
        }
    }
}
