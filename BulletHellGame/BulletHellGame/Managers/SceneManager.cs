namespace BulletHellGame.Managers
{
    public class SceneManager
    {
        private static SceneManager _instance;
        public static SceneManager Instance => _instance ??= new SceneManager();

        private readonly Stack<IScene> _sceneStack = new Stack<IScene>();
        private readonly Dictionary<Transitions, Transition> _transitions = new Dictionary<Transitions, Transition>();

        private Transition _activeTransition;
        private bool _isTransitioning = false;
        private IScene _newScene;

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

        public void AddScene(IScene newScene, Transitions transition = Transitions.None, float duration = 0.5f)
        {
            if (_isTransitioning) return;
            newScene.Load();

            if (transition != Transitions.None)
            {
                _isTransitioning = true;
                _newScene = newScene;
                _activeTransition = _transitions[transition];
                _activeTransition.Start(_sceneStack.Peek().GetFrame(), newScene.GetFrame(), duration);
            }
            else
            {
                // No transition; just push the scene immediately
                _sceneStack.Push(newScene);
            }
        }

        public void RemoveScene(Transitions transition = Transitions.None, float duration = 0.5f)
        {
            if (_isTransitioning || _sceneStack.Count == 0) return;

            if (transition != Transitions.None)
            {
                _isTransitioning = true;
                _newScene = _sceneStack.Count > 1 ? _sceneStack.Peek() : null;
                _activeTransition = _transitions[transition];
                _activeTransition.Start(_sceneStack.Peek().GetFrame(), _newScene.GetFrame(), duration);
            }
            else
            {
                // No transition; just remove the scene immediately
                _sceneStack.Pop();
            }
        }

        public IScene GetCurrentScene()
        {
            return _sceneStack.Count > 0 ? _sceneStack.Peek() : null;
        }

        public void Update(GameTime gameTime)
        {
            if (_isTransitioning)
            {
                // Update the active transition
                if (_activeTransition.Update(gameTime))
                {
                    // Transition finished
                    if (_newScene != null)
                    {
                        _sceneStack.Push(_newScene);
                        _newScene = null;
                    }
                    else
                    {
                        _sceneStack.Pop();
                    }

                    _isTransitioning = false;
                    _activeTransition = null;
                }
            }
            else
            {
                // Update the current scene
                _sceneStack.Peek()?.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_isTransitioning)
            {
                // Draw transition
                _activeTransition.Draw(spriteBatch);

                if (_sceneStack.Count > 0)
                {
                    _sceneStack.Peek()?.Draw(spriteBatch);
                }
            }
            else
            {
                // Draw the current scene
                _sceneStack.Peek()?.Draw(spriteBatch);
            }
        }
    }
}
