using System.IO;
using System.Text.Json;
using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.DataLoaders
{
    public class CharacterDataLoader
    {
        private static string CHARACTERS_PATH = "Data/Characters"; // Folder containing character JSONs
        private static string SHOT_TYPES_PATH = "Data/ShotTypes"; // Folder containing shot type JSONs

        public static CharacterData LoadCharacterData(string characterId)
        {
            string characterFilePath = Path.Combine(CHARACTERS_PATH, $"{characterId}.json");
            if (!File.Exists(characterFilePath))
                throw new FileNotFoundException($"Character file not found: {characterFilePath}");

            string characterJson = File.ReadAllText(characterFilePath);
            var character = JsonSerializer.Deserialize<CharacterData>(characterJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (character == null)
                throw new Exception("Failed to deserialize character data.");

            // Load shot type data
            //character.ShotTypeDescriptions = LoadShotTypes(character.ShotTypes);

            return character;
        }

        private static List<ShotTypeDescription> LoadShotTypes(List<string> shotTypeNames)
        {
            var shotTypes = new List<ShotTypeDescription>();
            foreach (var shotType in shotTypeNames)
            {
                string shotTypeFilePath = Path.Combine(SHOT_TYPES_PATH, $"{shotType}.json");
                if (!File.Exists(shotTypeFilePath))
                {
                    Console.WriteLine($"Warning: Shot type file not found: {shotTypeFilePath}");
                    continue;
                }

                string shotTypeJson = File.ReadAllText(shotTypeFilePath);
                var shotTypeData = JsonSerializer.Deserialize<ShotTypeDescription>(shotTypeJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (shotTypeData != null)
                {
                    shotTypes.Add(shotTypeData);
                }
            }
            return shotTypes;
        }
    }
}
