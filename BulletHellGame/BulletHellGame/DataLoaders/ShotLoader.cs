//using System.IO;
//using System.Text.Json;
//using BulletHellGame.Data.DataTransferObjects;
//using BulletHellGame.Entities;

//namespace BulletHellGame.DataLoaders
//{
//    public class ShotLoader
//    {
//        private const string ShotDataDirectory = "Data/Shots/";

//        public static ShotData LoadShotData(string shotName)
//        {
//            string filePath = Path.Combine(ShotDataDirectory, $"{shotName}.json");

//            if (!File.Exists(filePath))
//            {
//                Console.WriteLine($"Error: Shot data file not found at {filePath}");
//                return null;
//            }

//            try
//            {
//                string json = File.ReadAllText(filePath);
//                var shotDto = JsonSerializer.Deserialize<ShotJsonDto>(json);

//                if (shotDto == null)
//                    throw new Exception("Failed to deserialize shot data.");

//                return ConvertToShotData(shotDto);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error loading shot data: {ex.Message}");
//                return null;
//            }
//        }

//        private static ShotData ConvertToShotData(ShotJsonDto dto)
//        {
//            var shotData = new ShotData
//            {
//                Name = dto.Name,
//                Description = dto.Description,
//                PowerLevels = new Dictionary<int, PowerLevelData>()
//            };

//            foreach (var powerLevel in dto.PowerLevels)
//            {
//                var powerLevelData = new PowerLevelData
//                {
//                    MainWeapons = new List<WeaponData>(), // Initialize the list
//                    Options = new List<OptionData>()
//                };

//                // Convert and add all main weapons
//                foreach (var mainWeaponDto in powerLevel.MainWeapons)
//                {
//                    powerLevelData.MainWeapons.Add(ConvertToWeaponData(mainWeaponDto));
//                }

//                // Convert and add all side weapons
//                foreach (var sideWeaponDto in powerLevel.SideWeapons)
//                {
//                    Vector2 offset = new Vector2(sideWeaponDto.Offset.X, sideWeaponDto.Offset.Y);
//                    OptionData weapon = ConvertToWeaponData(sideWeaponDto);
//                    weapon.Offset = offset;
//                    powerLevelData.Options.Add(weapon);
//                }

//                shotData.PowerLevels[powerLevel.Level] = powerLevelData;
//            }

//            return shotData;
//        }


//        private static WeaponData ConvertToWeaponData(WeaponJsonDto dto)
//        {
//            return new WeaponData
//            {
//                FireRate = dto.FireRate,
//                FireDirections = ConvertToVector2List(dto.FireDirections),
//                BulletData = new BulletData
//                {
//                    SpriteName = dto.BulletData.SpriteName,
//                    Damage = dto.BulletData.Damage,
//                    BulletType = Enum.TryParse(dto.BulletData.BulletType, true, out BulletType bulletType)
//                        ? bulletType
//                        : throw new Exception($"Invalid BulletType: {dto.BulletData.BulletType}")
//                }
//            };
//        }

//        private static OptionData ConvertToOptionData(OptionJsonDto dto) { }

//        private static List<Vector2> ConvertToVector2List(List<Vector2Dto> list)
//        {
//            List<Vector2> vectors = new();
//            foreach (var v in list)
//            {
//                vectors.Add(new Vector2(v.X, v.Y));
//            }
//            return vectors;
//        }
//    }
//}
