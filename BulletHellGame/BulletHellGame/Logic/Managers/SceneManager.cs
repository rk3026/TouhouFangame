using BulletHellGame.Presentation.Scenes;
using System.Linq;

namespace BulletHellGame.Logic.Managers
{
    public class SceneManager
    {
        private static SceneManager _instance;
        public static SceneManager Instance => _instance ??= new SceneManager();

        private readonly Stack<IScene> _sceneStack = new Stack<IScene>();
        public Action SceneChanged;

        public IReadOnlyCollection<IScene> SceneStack => _sceneStack;


        private SceneManager() { }

        public void AddScene(IScene newScene)
        {
            newScene.Load();
            _sceneStack.Push(newScene);
            SceneChanged?.Invoke();
        }

        public void RemoveScene()
        {
            if (_sceneStack.Count == 0) return;
            _sceneStack.Pop();
            SceneChanged?.Invoke();
        }

        public void ClearScenes()
        {
            _sceneStack.Clear();
            SceneChanged?.Invoke();
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
            int drawStartIndex = 0;

            // Start from the top and go downwards to find the first non-overlay scene
            while (drawStartIndex < _sceneStack.Count - 1 && _sceneStack.ElementAt(drawStartIndex).IsOverlay)
            {
                drawStartIndex++;
            }

            // Draw all scenes from that point up to the current scene
            for (int i = drawStartIndex; i >= 0; i--)
            {
                _sceneStack.ElementAt(i)?.Draw(spriteBatch);
            }

            if (SettingsManager.Instance.Debugging) DrawSceneStackDebug(spriteBatch);
        }



        private void DrawSceneStackDebug(SpriteBatch spriteBatch)
        {
            // Debugging: Draw the scene stack count and the scene names
            string debugText = $"Scene Stack Count: {SceneStack.Count}";
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
