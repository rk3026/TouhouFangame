using System.IO;
using BulletHellGame.Data.DataTransferObjects;
using Newtonsoft.Json;

namespace BulletHellGame.Managers
{
    public static class LevelManager
    {
        public static LevelData LoadLevel(string levelName)
        {
            // Construct the full path for the level file
            string levelPath = Path.Combine("Data", "LevelData", levelName + ".json");

            // Check if the file exists at the constructed path
            if (!File.Exists(levelPath))
            {
                throw new FileNotFoundException($"Level file not found: {levelPath}");
            }

            // Read the JSON file content
            string json = File.ReadAllText(levelPath);

            // Deserialize JSON into the LevelData object
            return JsonConvert.DeserializeObject<LevelData>(json);
        }
    }
}
