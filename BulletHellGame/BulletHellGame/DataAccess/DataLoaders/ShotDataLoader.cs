using System.IO;
using System.Linq;
using System.Text.Json;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;

namespace BulletHellGame.DataAccess.DataLoaders
{
    public static class ShotDataLoader
    {
        private const string ShotDataDirectory = "Data/Shots/";

        public static ShotData LoadFromJson(string shotName)
        {
            try
            {
                string filePath = Path.Combine(ShotDataDirectory, $"{shotName.Replace(" ", "")}.json");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Shot data file not found: {filePath}");
                    return null;
                }

                string json = File.ReadAllText(filePath);
                using JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;

                var powerLevels = new Dictionary<int, PowerLevelData>();

                foreach (var level in root.GetProperty("PowerLevels").EnumerateArray())
                {
                    int levelNumber = level.GetProperty("Level").GetInt32();
                    powerLevels[levelNumber] = new PowerLevelData
                    {
                        MainWeapons = level.GetProperty("MainWeapons").EnumerateArray()
                            .Select(ParseWeapon).ToList(),
                        Options = level.TryGetProperty("SideWeapons", out JsonElement sideWeapons)
                            ? sideWeapons.EnumerateArray().Select(ParseOption).ToList()
                            : new List<SpawnerData>()
                    };
                }

                // Fill missing levels by interpolating from nearest defined levels
                int maxLevel = powerLevels.Keys.Max();
                for (int i = 1; i <= maxLevel; i++)
                {
                    if (!powerLevels.ContainsKey(i))
                    {
                        // Find the closest previous and next defined levels
                        int prevLevel = powerLevels.Keys.Where(l => l < i).DefaultIfEmpty(1).Max();
                        int nextLevel = powerLevels.Keys.Where(l => l > i).DefaultIfEmpty(maxLevel).Min();

                        // Copy the data from the closest previous level (or interpolate if needed)
                        powerLevels[i] = new PowerLevelData
                        {
                            MainWeapons = powerLevels[prevLevel].MainWeapons,
                            Options = powerLevels[prevLevel].Options
                        };
                    }
                }

                return new ShotData
                {
                    Name = root.GetProperty("Name").GetString(),
                    Description = root.GetProperty("Description").GetString(),
                    PowerLevels = powerLevels
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load shot data for '{shotName}': {ex.Message}");
                return null;
            }
        }



        private static WeaponData ParseWeapon(JsonElement weaponElement)
        {
            return new WeaponData
            {
                FireRate = weaponElement.GetProperty("FireRate").GetSingle(),
                FireDirections = weaponElement.GetProperty("FireDirections").EnumerateArray()
                    .Select(ParseVector2).ToList(),
                BulletData = ParseBullet(weaponElement.GetProperty("BulletData"))
            };
        }

        private static SpawnerData ParseOption(JsonElement optionElement)
        {
            return new SpawnerData
            {
                Weapons = new List<WeaponData>
                {
                    new WeaponData
                    {
                        FireRate = optionElement.GetProperty("FireRate").GetSingle(),
                        FireDirections = optionElement.GetProperty("FireDirections").EnumerateArray()
                            .Select(ParseVector2).ToList(),
                        BulletData = ParseBullet(optionElement.GetProperty("BulletData"))
                    }
                },
                SpriteName = optionElement.GetProperty("SpriteName").GetString(),
                Offset = ParseVector2(optionElement.GetProperty("Offset"))
            };
        }

        private static BulletData ParseBullet(JsonElement bulletElement)
        {
            return new BulletData
            {
                SpriteName = bulletElement.GetProperty("SpriteName").GetString(),
                Damage = bulletElement.GetProperty("Damage").GetInt32(),
                BulletType = Enum.Parse<BulletType>(bulletElement.GetProperty("BulletType").GetString())
            };
        }

        private static Vector2 ParseVector2(JsonElement vectorElement)
        {
            return new Vector2(
                vectorElement.GetProperty("X").GetSingle(),
                vectorElement.GetProperty("Y").GetSingle()
            );
        }
    }
}
