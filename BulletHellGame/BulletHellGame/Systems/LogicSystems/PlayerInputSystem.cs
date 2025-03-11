using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System.Linq;

namespace BulletHellGame.Systems.LogicSystems
{
    public class PlayerInputSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity player in entityManager.GetEntitiesWithComponents(typeof(PlayerInputComponent)))
            {
                if (player.TryGetComponent<PlayerInputComponent>(out var pic) &&
                    player.TryGetComponent<PositionComponent>(out var pc) &&
                    player.TryGetComponent<VelocityComponent>(out var vc) &&
                    player.TryGetComponent<SpriteComponent>(out var sc) &&
                    player.TryGetComponent<SpeedComponent>(out var speedComponent) &&
                    player.TryGetComponent<ShootingComponent>(out var shootingComponent) &&
                    player.TryGetComponent<PowerLevelComponent>(out var powerLevelComponent) &&
                    player.TryGetComponent<PlayerStatsComponent>(out var psc)
                    )
                {
                    // Movement input
                    pic.IsMovingUp = InputManager.Instance.ActionDown(GameAction.MoveUp);
                    pic.IsMovingDown = InputManager.Instance.ActionDown(GameAction.MoveDown);
                    pic.IsMovingLeft = InputManager.Instance.ActionDown(GameAction.MoveLeft);
                    pic.IsMovingRight = InputManager.Instance.ActionDown(GameAction.MoveRight);

                    // Action input
                    pic.IsShooting = InputManager.Instance.ActionDown(GameAction.Shoot);

                    // Special mode (focused mode)
                    pic.IsFocused = InputManager.Instance.ActionDown(GameAction.Slow);

                    // Handle movement based on the input states
                    Vector2 direction = Vector2.Zero;
                    if (pic.IsMovingUp) direction.Y -= 1;
                    if (pic.IsMovingDown) direction.Y += 1;
                    if (pic.IsMovingLeft) direction.X -= 1;
                    if (pic.IsMovingRight) direction.X += 1;

                    if (direction.LengthSquared() > 0)
                        direction.Normalize();

                    // Adjust speed based on whether the player is focused
                    float currentSpeed = pic.IsFocused ? speedComponent.FocusedSpeed : speedComponent.Speed;
                    vc.Velocity = direction * currentSpeed;

                    // Ensure the player stays within bounds
                    float halfWidth = sc.CurrentFrame.Width / 2f;
                    float halfHeight = sc.CurrentFrame.Height / 2f;
                    pc.Position = new Vector2(
                        Math.Clamp(pc.Position.X, entityManager.Bounds.Left + halfWidth, entityManager.Bounds.Right - halfWidth),
                        Math.Clamp(pc.Position.Y, entityManager.Bounds.Top + halfHeight, entityManager.Bounds.Bottom - halfHeight)
                    );

                    // Weapon Switching Logic (Player & Options)
                    UpdateShootingModes(player, entityManager, pic.IsFocused, psc, powerLevelComponent);
                }
            }
        }

        private void UpdateShootingModes(Entity player, EntityManager entityManager, bool isFocused, PlayerStatsComponent psc, PowerLevelComponent plc)
        {
            int currentPowerLevel = psc.CurrentPowerLevel;

            // Get the correct weapon data based on focused/unfocused mode
            var mainWeapons = isFocused && plc.FocusedPowerLevels.ContainsKey(currentPowerLevel)
                ? plc.FocusedPowerLevels[currentPowerLevel].MainWeapons
                : plc.UnfocusedPowerLevels.ContainsKey(currentPowerLevel)
                    ? plc.UnfocusedPowerLevels[currentPowerLevel].MainWeapons
                    : new List<WeaponData>(); // Default to empty list if not found

            var optionWeapons = isFocused && plc.FocusedPowerLevels.ContainsKey(currentPowerLevel)
                ? plc.FocusedPowerLevels[currentPowerLevel].Options
                : plc.UnfocusedPowerLevels.ContainsKey(currentPowerLevel)
                    ? plc.UnfocusedPowerLevels[currentPowerLevel].Options
                    : new List<OptionData>(); // Default to empty list if not found

            // Update the player's shooting component
            if (player.TryGetComponent<ShootingComponent>(out var playerShooting))
            {
                playerShooting.SetWeapons(mainWeapons);
            }

            // Get all options owned by the player
            List<Entity> options = entityManager.GetEntitiesWithComponents(typeof(OwnerComponent), typeof(ShootingComponent))
                .Where(option => option.TryGetComponent<OwnerComponent>(out var owner) && owner.Owner == player)
                .ToList();

            for (int i = 0; i < options.Count; i++)
            {
                if (options[i].TryGetComponent<ShootingComponent>(out var optionShooting))
                {
                    List<WeaponData> optionWeaponData = (i < optionWeapons.Count) ? optionWeapons[i].Weapons : new List<WeaponData>();
                    optionShooting.SetWeapons(optionWeaponData);
                }
            }
        }

    }
}
