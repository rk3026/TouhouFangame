using System.IO;
using System.Text.Json;
using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.DataAccess.DataLoaders
{
    public class ShotTypeLoader
    {
        private static string SHOT_TYPES_PATH = "Data/ShotTypes"; // Folder containing shot id JSONs

        public static ShotTypeData LoadShotTypes(string shotTypeName)
        {
            string shotTypeFilePath = Path.Combine(SHOT_TYPES_PATH, $"{shotTypeName}.json");

            if (!File.Exists(shotTypeFilePath))
            {
                throw new FileNotFoundException($"Shot id file not found: {shotTypeFilePath}");
            }

            string json = File.ReadAllText(shotTypeFilePath);
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement root = doc.RootElement;

            ShotTypeData shotTypeData = new ShotTypeData
            {
                Name = shotTypeName,
                UnfocusedShot = ShotDataLoader.LoadFromJson(root.GetProperty("unfocused_shot").GetString()),
                FocusedShot = ShotDataLoader.LoadFromJson(root.GetProperty("focused_shot").GetString()),
                //UnfocusedBomb = BombDataLoader.LoadFromJson(root.GetProperty("unfocused_bomb").GetString()),
                //FocusedBomb = BombDataLoader.LoadFromJson(root.GetProperty("focused_bomb").GetString())
            };

            return shotTypeData;
        }
    }
}
