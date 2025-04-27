using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Managers
{
    public class ScreenFlipManager
    {
        private SpriteEffects _currentFlip = SpriteEffects.None;
        private SpriteEffects _lastFlip = SpriteEffects.None;

        private Rectangle _playableArea;

        public ScreenFlipManager(Rectangle playableArea)
        {
            _playableArea = playableArea;
        }

        public SpriteEffects CurrentFlip => _currentFlip;

        public void Update(EntityManager entityManager)
        {
            // Input handling
            if (InputManager.Instance.ActionPressed(GameAction.MenuUp))
                _currentFlip ^= SpriteEffects.FlipVertically;

            if (InputManager.Instance.ActionPressed(GameAction.MenuLeft))
                _currentFlip ^= SpriteEffects.FlipHorizontally;

            if (_currentFlip == _lastFlip)
                return;

            bool flipX = (_currentFlip & SpriteEffects.FlipHorizontally) != (_lastFlip & SpriteEffects.FlipHorizontally);
            bool flipY = (_currentFlip & SpriteEffects.FlipVertically) != (_lastFlip & SpriteEffects.FlipVertically);

            float centerX = (_playableArea.X + _playableArea.Width) / 2f;
            float centerY = (_playableArea.Y + _playableArea.Height) / 2f;

            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(PositionComponent), typeof(VelocityComponent), typeof(SpriteComponent)))
            {
                var pc = entity.GetComponent<PositionComponent>();
                var vc = entity.GetComponent<VelocityComponent>();
                var sc = entity.GetComponent<SpriteComponent>();

                sc.SpriteEffect = _currentFlip;

                if (flipX)
                {
                    pc.Position = new Vector2(2 * centerX - pc.Position.X, pc.Position.Y);
                    vc.Velocity = new Vector2(-vc.Velocity.X, vc.Velocity.Y);
                }

                if (flipY)
                {
                    pc.Position = new Vector2(pc.Position.X, 2 * centerY - pc.Position.Y);
                    vc.Velocity = new Vector2(vc.Velocity.X, -vc.Velocity.Y);
                }
            }

            foreach (Entity entity in entityManager.GetEntitiesWithComponents(typeof(ShootingComponent)))
            {
                var shooting = entity.GetComponent<ShootingComponent>();

                foreach (var weapon in shooting.Weapons)
                {
                    for (int i = 0; i < weapon.FireDirections.Count; i++)
                    {
                        var dir = weapon.FireDirections[i];

                        if (flipX) dir.X *= -1;
                        if (flipY) dir.Y *= -1;

                        weapon.FireDirections[i] = dir;
                    }
                }
            }

            _lastFlip = _currentFlip;
        }
    }

}
