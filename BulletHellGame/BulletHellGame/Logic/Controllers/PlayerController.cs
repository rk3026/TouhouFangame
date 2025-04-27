using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Components;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Managers;
using System.Linq;

namespace BulletHellGame.Logic.Controllers
{
    public class PlayerController : EntityController
    {
        public override void Update(EntityManager entityManager, Entity entity)
        {
            var inputManager = InputManager.Instance;

            // Grab components
            if (!(entity.TryGetComponent<PositionComponent>(out var pc) &&
                  entity.TryGetComponent<VelocityComponent>(out var vc) &&
                  entity.TryGetComponent<SpriteComponent>(out var sc) &&
                  entity.TryGetComponent<SpeedComponent>(out var speedComponent) &&
                  entity.TryGetComponent<PowerLevelComponent>(out var powerLevelComponent) &&
                  entity.TryGetComponent<PlayerStatsComponent>(out var psc)))
            {
                return;
            }

            // Handle movement input
            Vector2 direction = Vector2.Zero;
            if (inputManager.ActionDown(GameAction.MoveUp)) direction.Y -= 1;
            if (inputManager.ActionDown(GameAction.MoveDown)) direction.Y += 1;
            if (inputManager.ActionDown(GameAction.MoveLeft)) direction.X -= 1;
            if (inputManager.ActionDown(GameAction.MoveRight)) direction.X += 1;

            if (direction.LengthSquared() > 0)
                direction.Normalize();

            // Adjust speed if focused
            bool isFocused = inputManager.ActionDown(GameAction.Slow);
            float currentSpeed = isFocused ? speedComponent.FocusedSpeed : speedComponent.Speed;
            vc.Velocity = direction * currentSpeed;

            // Clamp position to stay in bounds
            float halfWidth = sc.CurrentFrame.Width / 2f;
            float halfHeight = sc.CurrentFrame.Height / 2f;
            pc.Position = new Vector2(
                Math.Clamp(pc.Position.X, entityManager.Bounds.Left + halfWidth, entityManager.Bounds.Right - halfWidth),
                Math.Clamp(pc.Position.Y, entityManager.Bounds.Top + halfHeight, entityManager.Bounds.Bottom - halfHeight)
            );

            // Shooting input
            this.IsShooting = inputManager.ActionDown(GameAction.Shoot);
            this.IsBombing = inputManager.ActionDown(GameAction.Bomb);
            this.IsMoving = direction.LengthSquared() > 0;
            this.Direction = (float)Math.Atan2(direction.Y, direction.X);

            // Power level switching logic
            UpdateShootingModes(entity, entityManager, isFocused, psc, powerLevelComponent);
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
                    : new List<SpawnerData>(); // Default to empty list if not found

            // Find the player's spawner and update its weapons
            var playerSpawner = entityManager.GetEntitiesWithComponents(typeof(OwnerComponent), typeof(ShootingComponent))
                .FirstOrDefault(e =>
                    e.TryGetComponent<OwnerComponent>(out var owner) && owner.Owner == player);

            if (playerSpawner != null && playerSpawner.TryGetComponent<ShootingComponent>(out var spawnerShootingComponent))
            {
                spawnerShootingComponent.SetWeapons(mainWeapons);
            }

            // Find all option spawners (spawners whose owner is an option of the player)
            var optionSpawners = entityManager.GetEntitiesWithComponents(typeof(OwnerComponent), typeof(ShootingComponent))
                .Where(e =>
                    e.TryGetComponent<OwnerComponent>(out var owner) &&
                    owner.Owner != null &&
                    owner.Owner.TryGetComponent<OwnerComponent>(out var optionOwner) &&
                    optionOwner.Owner == player)
                .ToList();

            for (int i = 0; i < optionSpawners.Count; i++)
            {
                if (optionSpawners[i].TryGetComponent<ShootingComponent>(out var optionSpawnerComponent))
                {
                    List<WeaponData> optionWeaponData = i < optionWeapons.Count ? optionWeapons[i].Weapons : new List<WeaponData>();
                    optionSpawnerComponent.SetWeapons(optionWeaponData);
                }
            }
        }


    }
}
