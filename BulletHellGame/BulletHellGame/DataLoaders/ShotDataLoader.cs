using System.IO;
using System.Linq;
using System.Text.Json;
using BulletHellGame.Data.DataTransferObjects;
using BulletHellGame.Entities;

namespace BulletHellGame.Data.Loaders
{
    public static class ShotDataLoader
    {
        private const string ShotDataDirectory = "Data/Shots/";

        public static ShotData LoadFromJson(string shotName)
        {
            string filePath = Path.Combine(ShotDataDirectory, $"{shotName}.json");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Shot data file not found: {filePath}");

            string json = File.ReadAllText(filePath);
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            return new ShotData
            {
                Name = root.GetProperty("Name").GetString(),
                Description = root.GetProperty("Description").GetString(),
                PowerLevels = root.GetProperty("UnfocusedPowerLevels").EnumerateArray()
                    .ToDictionary(
                        level => level.GetProperty("Level").GetInt32(),
                        level => new PowerLevelData
                        {
                            MainWeapons = level.GetProperty("MainWeapons").EnumerateArray()
                                .Select(ParseWeapon).ToList(),
                            Options = level.TryGetProperty("SideWeapons", out JsonElement sideWeapons)
                                ? sideWeapons.EnumerateArray().Select(ParseOption).ToList()
                                : new List<OptionData>()
                        }
                    )
            };
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

        private static OptionData ParseOption(JsonElement optionElement)
        {
            return new OptionData
            {
                SpriteName = optionElement.GetProperty("SpriteName").GetString(),
                Offset = ParseVector2(optionElement.GetProperty("Offset")),
                Weapons = new List<WeaponData>
                {
                    new WeaponData
                    {
                        FireRate = optionElement.GetProperty("FireRate").GetSingle(),
                        FireDirections = optionElement.GetProperty("FireDirections").EnumerateArray()
                            .Select(ParseVector2).ToList(),
                        BulletData = ParseBullet(optionElement.GetProperty("BulletData"))
                    }
                }
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
