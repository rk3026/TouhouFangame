using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Components
{
    public class ShootingComponent : IComponent
    {
        public bool IsShooting { get; set; } = false;

        public List<WeaponData> Weapons { get; private set; } = new List<WeaponData>();

        // Stores cooldowns per weapon
        public Dictionary<WeaponData, float> WeaponCooldowns { get; private set; } = new Dictionary<WeaponData, float>();

        public ShootingComponent(List<WeaponData> weapons)
        {
            SetWeapons(weapons);
        }

        public void SetWeapons(List<WeaponData> newWeapons)
        {
            Weapons = newWeapons;

            // Ensure cooldowns exist for each weapon
            foreach (var weapon in Weapons)
            {
                if (!WeaponCooldowns.ContainsKey(weapon))
                {
                    WeaponCooldowns[weapon] = 0f; // Start with no cooldown
                }
            }
        }
    }
}
