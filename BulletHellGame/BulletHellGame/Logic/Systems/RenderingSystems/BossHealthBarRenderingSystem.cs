﻿using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Managers;

namespace BulletHellGame.Logic.Systems.RenderingSystems
{
    public class BossHealthBarRenderingSystem : IRenderingSystem
    {
        public int DrawPriority => 2;

        private GraphicsDevice _graphicsDevice;
        private Texture2D _barTexture;
        private float _displayedHealthPercent = 0f;
        private float _fillSpeed = 1.0f; // Controls the GAME_SPEED of the fill-up effect
        private SpriteFont _font;

        public BossHealthBarRenderingSystem(GraphicsDevice gd)
        {
            _graphicsDevice = gd;
            _font = FontManager.Instance.GetFont("DFPPOPCorn-W12");
            _barTexture = new Texture2D(_graphicsDevice, 1, 1);
            _barTexture.SetData(new Color[] { Color.White });
        }

        public void Draw(EntityManager entityManager, SpriteBatch spriteBatch)
        {
            foreach (var entity in entityManager.GetEntitiesWithComponents(typeof(HealthComponent), typeof(PositionComponent), typeof(IndicatorComponent), typeof(BossPhaseComponent)))
            {
                var health = entity.GetComponent<HealthComponent>();
                var phaseComponent = entity.GetComponent<BossPhaseComponent>();

                float targetHealthPercent = (float)health.CurrentHealth / health.MaxHealth;

                // Gradually increase displayed health towards the actual health
                _displayedHealthPercent += _fillSpeed * 1 / 60f; // Assuming 60 FPS
                _displayedHealthPercent = MathHelper.Clamp(_displayedHealthPercent, 0f, targetHealthPercent);

                // Draw within entityManager.Bounds with slight offset
                var bounds = entityManager.Bounds;
                int offset = 30;
                int healthBarWidth = (int)((bounds.Width - 2 * offset) * _displayedHealthPercent);
                int healthBarHeight = 5;
                Vector2 healthBarPosition = new Vector2(bounds.X + offset, bounds.Y + healthBarHeight + 5);

                Color backgroundColor = Color.DarkRed * 0.7f;
                Color foregroundColor = Color.Red * 0.7f;

                // Background bar
                spriteBatch.Draw(_barTexture, new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, bounds.Width - 2 * offset, healthBarHeight), backgroundColor);
                // Foreground bar
                spriteBatch.Draw(_barTexture, new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, healthBarWidth, healthBarHeight), foregroundColor);

                // Draw Phase Text next to the health bar
                string phaseText = $"{phaseComponent.Phases[phaseComponent.CurrentPhase].Name}({phaseComponent.CurrentPhase + 1}/{phaseComponent.Phases.Count})";
                Vector2 phaseTextPosition = new Vector2(bounds.X + offset, healthBarPosition.Y + healthBarHeight + 5);
                spriteBatch.DrawString(_font, phaseText, phaseTextPosition, Color.White);
            }
        }
    }
}
