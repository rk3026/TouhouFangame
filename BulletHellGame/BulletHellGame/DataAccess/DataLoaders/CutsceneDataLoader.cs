using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellGame.DataAccess.DataLoaders
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using static BulletHellGame.DataAccess.DataLoaders.CharacterDataLoader;

    public static class CutsceneDataLoader
    {
        private static readonly string CUTSCENE_PATH = "Content/Data/Cutscenes";

        public static List<Cutscene> LoadCutscenesForLevel(int level)
        {
            try
            {
                string cutsceneFilePath = Path.Combine(CUTSCENE_PATH, $"Level{level}.json");

                if (!File.Exists(cutsceneFilePath))
                    throw new FileNotFoundException($"Cutscene file not found: {cutsceneFilePath}");

                string json = File.ReadAllText(cutsceneFilePath);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new Vector2Converter()); // If you use Vector2 in cutscene data

                List<CutsceneDto> cutsceneDtos = JsonSerializer.Deserialize<List<CutsceneDto>>(json, options);

                List<Cutscene> cutscenes = new();
                foreach (var dto in cutsceneDtos)
                {
                    List<DialogueLine> dialogue = new();
                    foreach (var line in dto.Dialogue)
                    {
                        dialogue.Add(new DialogueLine(line.Speaker, line.Line, line.Expression));
                    }

                    cutscenes.Add(new Cutscene(dto.BackgroundAsset, dto.MusicAsset, dto.SpriteAsset, dialogue));
                }

                return cutscenes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading cutscenes for level {level}: {ex.Message}");
                return new List<Cutscene>();
            }
        }

        public class CutsceneDto
        {
            public string BackgroundAsset { get; set; }
            public string MusicAsset { get; set; }
            public string SpriteAsset { get; set; }
            public List<DialogueLineDto> Dialogue { get; set; }
        }

        public class DialogueLineDto
        {
            public string Speaker { get; set; }
            public string Line { get; set; }
            public string Expression { get; set; }
        }
    }



}
