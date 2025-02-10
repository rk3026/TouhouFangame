using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;

namespace BulletHellGame.Systems.LogicSystems
{
    public class PowerLevelSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity player in entityManager.GetEntitiesWithComponents(typeof(PlayerStatsComponent), typeof(ShootingLevelComponent)))
            {
                if (!player.TryGetComponent<PlayerStatsComponent>(out var psc)) continue;
                if (!player.TryGetComponent<ShootingLevelComponent>(out var plc)) continue;

                // Determine the correct power level
                int newPowerLevel = DeterminePowerLevel(psc.Power);

                // If the power level has changed, update shooting components
                if (psc.CurrentPowerLevel != newPowerLevel)
                {
                    psc.CurrentPowerLevel = newPowerLevel;
                    UpdateShooting(player, entityManager, psc, plc);
                }
            }
        }

        private int DeterminePowerLevel(float power)
        {
            if (power >= 128) return 8;
            if (power >= 96) return 7;
            if (power >= 80) return 6;
            if (power >= 64) return 5;
            if (power >= 48) return 4;
            if (power >= 32) return 3;
            if (power >= 16) return 2;
            if (power >= 8) return 1;
            return 0;
        }

        private void UpdateShooting(Entity player, EntityManager entityManager, PlayerStatsComponent psc, ShootingLevelComponent slc)
        {

            // Update the player's shooting component
            if (player.TryGetComponent<ShootingComponent>(out var playerShooting))
            {
                ApplyWeaponDataToShooting(playerShooting, slc.PowerLevels[psc.CurrentPowerLevel]);
            }

            // Update all weapons owned by the player
            List<Entity> weapons = entityManager.GetEntitiesWithComponents(typeof(OwnerComponent));

            foreach (Entity weapon in weapons)
            {
                if (weapon.TryGetComponent<OwnerComponent>(out var owner) && owner.Owner == player)
                {
                    if (weapon.TryGetComponent<ShootingComponent>(out var weaponShooting) &&
                        weapon.TryGetComponent<ShootingLevelComponent>(out var weaponslc))
                    {
                        ApplyWeaponDataToShooting(weaponShooting, weaponslc.PowerLevels[psc.CurrentPowerLevel]);
                    }
                }
            }
        }

        private void ApplyWeaponDataToShooting(ShootingComponent shootingComponent, WeaponData weaponData)
        {
            shootingComponent.BulletData = weaponData.BulletData;
            shootingComponent.FireRate = weaponData.FireRate;
            shootingComponent.FireDirections = weaponData.FireDirections;
            shootingComponent.TimeSinceLastShot = 0f; // Reset firing cooldown
        }
    }
}
