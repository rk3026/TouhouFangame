using BulletHellGame.Components;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;
using BulletHellGame.Managers;
using System.Linq;

namespace BulletHellGame.Systems.LogicSystems
{
    public class PowerLevelSystem : ILogicSystem
    {
        public void Update(EntityManager entityManager, GameTime gameTime)
        {
            foreach (Entity player in entityManager.GetEntitiesWithComponents(typeof(PlayerStatsComponent), typeof(PowerLevelComponent)))
            {
                if (!player.TryGetComponent<PlayerStatsComponent>(out var psc)) continue;
                if (!player.TryGetComponent<PowerLevelComponent>(out var plc)) continue;

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

        private void UpdateShooting(Entity player, EntityManager entityManager, PlayerStatsComponent psc, PowerLevelComponent plc)
        {
            // Update the player's shooting component
            if (player.TryGetComponent<ShootingComponent>(out var playerShooting))
            {
                ApplyWeaponDataToShooting(playerShooting, plc.PowerLevels[psc.CurrentPowerLevel].MainWeapons);
            }

            // Get all options owned by the player
            List<Entity> options = entityManager.GetEntitiesWithComponents(typeof(OwnerComponent), typeof(ShootingComponent))
                .Where(option => option.TryGetComponent<OwnerComponent>(out var owner) && owner.Owner == player)
                .ToList();

            var optionDataList = plc.PowerLevels[psc.CurrentPowerLevel].Options;
            int optionCount = Math.Min(options.Count, optionDataList.Count);

            // Assign the correct weapon data to each option
            for (int i = 0; i < optionCount; i++)
            {
                if (options[i].TryGetComponent<ShootingComponent>(out var optionShooting))
                {
                    ApplyWeaponDataToShooting(optionShooting, optionDataList[i].Weapons);
                }
            }
        }

        private void ApplyWeaponDataToShooting(ShootingComponent sc, List<WeaponData> weaponDatas)
        {
            sc.SetWeapons(weaponDatas);
        }

    }
}
