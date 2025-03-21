using System.IO;
using System.Text.Json;
using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.DataAccess.DataLoaders
{
    public class CharacterDataLoader
    {
        private static string CHARACTERS_PATH = "Data/Characters"; // Folder containing character JSONs
        private static string SHOT_TYPES_PATH = "Data/ShotTypes"; // Folder containing shot type JSONs

        public static CharacterData LoadCharacterData(string characterName)
        {
            try
            {
                string characterFilePath = Path.Combine(CHARACTERS_PATH, $"{characterName}.json");

                if (!File.Exists(characterFilePath))
                {
                    throw new FileNotFoundException($"Character file not found: {characterFilePath}");
                }

                string json = File.ReadAllText(characterFilePath);

                // Deserialize into DTO first
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                options.Converters.Add(new Vector2Converter());
                CharacterDataDto characterDto = JsonSerializer.Deserialize<CharacterDataDto>(json, options);

                // Convert DTO to runtime CharacterData
                CharacterData characterData = new CharacterData
                {
                    Id = characterDto.Id,
                    Name = characterDto.Name,
                    Description = characterDto.Description,
                    SpriteName = characterDto.SpriteName,
                    Health = characterDto.Health,
                    HitboxSize = characterDto.HitboxSize,
                    MovementSpeed = characterDto.MovementSpeed,
                    FocusedSpeed = characterDto.FocusedSpeed,
                    InitialLives = characterDto.InitialLives,
                    InitialBombs = characterDto.InitialBombs,
                    BombCherryLoss = characterDto.BombCherryLoss,
                    DeathbombWindow = characterDto.DeathbombWindow,
                    SpecialAbilities = characterDto.SpecialAbilities,
                };
                characterData.ShotTypes = new List<ShotTypeData>();
                if (characterDto.ShotTypes != null)
                {
                    foreach (string shotType in characterDto.ShotTypes)
                    {
                        var loadedShotType = ShotTypeLoader.LoadShotTypes(shotType);
                        if (loadedShotType != null)
                        {
                            characterData.ShotTypes.Add(loadedShotType);
                        }
                    }
                }

                return characterData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading character data for '{characterName}': {ex.Message}");
                return null;
            }
        }


        public class CharacterDataDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string SpriteName { get; set; }
            public int Health { get; set; }
            public Vector2 HitboxSize { get; set; }
            public float MovementSpeed { get; set; }
            public float FocusedSpeed { get; set; }
            public int InitialLives { get; set; }
            public int InitialBombs { get; set; }
            public string BombCherryLoss { get; set; }
            public float DeathbombWindow { get; set; }
            public List<string> SpecialAbilities { get; set; } = new();
            public List<string> ShotTypes { get; set; }
        }

        public class Vector2Converter : System.Text.Json.Serialization.JsonConverter<Vector2>
        {
            public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var json = JsonDocument.ParseValue(ref reader).RootElement;
                float x = json.GetProperty("X").GetSingle();
                float y = json.GetProperty("Y").GetSingle();
                return new Vector2(x, y);
            }

            public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteNumber("X", value.X);
                writer.WriteNumber("Y", value.Y);
                writer.WriteEndObject();
            }
        }
    }
}