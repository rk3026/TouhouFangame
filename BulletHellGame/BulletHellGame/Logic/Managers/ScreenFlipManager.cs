namespace BulletHellGame.Logic.Managers
{
    public class ScreenFlipManager
    {
        private SpriteEffects _currentFlip = SpriteEffects.None;
        private SpriteEffects _targetFlip = SpriteEffects.None;

        private Rectangle _playableArea;

        private Vector2 _currentScale = Vector2.One; // Default scale is normal (1, 1)
        private Vector2 _targetScale = Vector2.One;  // Target scale for flipping
        private float _scaleSpeed = 5f; // Speed at which to scale (adjust as needed)

        private float _currentRotation = 0f; // Current rotation in radians (0 or PI)
        private float _targetRotation = 0f; // Target rotation (0 or PI)
        private float _rotationSpeed = MathHelper.Pi / 0.5f; // Speed for rotation (rotate 180 degrees in 0.5s)

        private bool _isFlipping = false;

        public ScreenFlipManager(Rectangle playableArea)
        {
            _playableArea = playableArea;
        }

        public SpriteEffects CurrentFlip => _currentFlip;

        public bool IsFlipping => _isFlipping; // If you ever want to know

        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            if (!_isFlipping)
            {
                if (InputManager.Instance.ActionPressed(GameAction.MenuUp))
                {
                    _targetFlip ^= SpriteEffects.FlipVertically;
                    _targetScale.Y = -_targetScale.Y; // Flip vertically by inverting Y scale
                    _isFlipping = true;
                }
                if (InputManager.Instance.ActionPressed(GameAction.MenuLeft))
                {
                    _targetFlip ^= SpriteEffects.FlipHorizontally;
                    _targetScale.X = -_targetScale.X; // Flip horizontally by inverting X scale
                    _isFlipping = true;
                }
                if (InputManager.Instance.ActionPressed(GameAction.MenuRight))
                {
                    // Toggle rotation flip (180 degrees)
                    _targetRotation += MathHelper.Pi; // Add 180 degrees (PI radians)
                    _isFlipping = true;
                }
            }
            else
            {
                // Animate scale
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 scaleStep = new Vector2(
                    Math.Sign(_targetScale.X - _currentScale.X) * _scaleSpeed * delta,
                    Math.Sign(_targetScale.Y - _currentScale.Y) * _scaleSpeed * delta
                );

                // Incrementally adjust the scale
                if (_currentScale.X != _targetScale.X)
                {
                    _currentScale.X = MathHelper.Clamp(_currentScale.X + scaleStep.X, -1f, 1f);
                }

                if (_currentScale.Y != _targetScale.Y)
                {
                    _currentScale.Y = MathHelper.Clamp(_currentScale.Y + scaleStep.Y, -1f, 1f);
                }

                // Animate rotation
                float rotationStep = _rotationSpeed * delta;
                if (_currentRotation < _targetRotation)
                {
                    _currentRotation = Math.Min(_currentRotation + rotationStep, _targetRotation);
                }
                else
                {
                    _currentRotation = Math.Max(_currentRotation - rotationStep, _targetRotation);
                }

                // If the scale and rotation have reached their targets, stop flipping
                if (Math.Abs(_currentScale.X - _targetScale.X) < 0.01f &&
                    Math.Abs(_currentScale.Y - _targetScale.Y) < 0.01f &&
                    Math.Abs(_currentRotation - _targetRotation) < 0.01f)
                {
                    _currentScale = _targetScale;
                    _currentRotation = _targetRotation;
                    _isFlipping = false;
                    _currentFlip = _targetFlip;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Action drawSystems, Action<SpriteBatch> drawBackground)
        {
            Vector2 center = new Vector2(_playableArea.X + _playableArea.Width / 2f, _playableArea.Y + _playableArea.Height / 2f);

            spriteBatch.End(); // Ensure you're not drawing anything yet

            // Apply scaling and rotation for screen flip
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                Matrix.CreateTranslation(-center.X, -center.Y, 0) *
                Matrix.CreateRotationZ(_currentRotation) *  // Apply rotation
                Matrix.CreateScale(_currentScale.X, _currentScale.Y, 1f) *  // Apply scaling for flip
                Matrix.CreateTranslation(center.X, center.Y, 0)
            );

            // Draw the background with the same transformation
            drawBackground(spriteBatch);

            // Now draw all systems with the flipped sprite batch context
            drawSystems();

            spriteBatch.End(); // End after custom drawing

            // Start drawing normally for the next pass
            spriteBatch.Begin();
        }
    }
}
