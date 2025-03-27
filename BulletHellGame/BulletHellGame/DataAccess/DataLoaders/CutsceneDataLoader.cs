using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class CutsceneDataLoader
{
    private static readonly string CUTSCENE_PATH = "Content/Data/Cutscenes";

    public static List<CutsceneData> LoadCutscenesForLevel(int level)
    {
        try
        {
            string filename = $"Level{level}Cutscenes.json";
            string cutsceneFilePath = Path.Combine(CUTSCENE_PATH, filename);

            if (!File.Exists(cutsceneFilePath))
                throw new FileNotFoundException($"Cutscene file not found: {cutsceneFilePath}");

            string json = File.ReadAllText(cutsceneFilePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<CutsceneDto> dtos = JsonSerializer.Deserialize<List<CutsceneDto>>(json, options);
            List<CutsceneData> cutscenes = new();

            foreach (var dto in dtos)
            {
                var dialogue = new List<DialogueLine>();
                if (dto.Dialogue != null)
                {
                    foreach (var lineDto in dto.Dialogue)
                    {
                        dialogue.Add(new DialogueLine
                        {
                            Speaker = lineDto.Speaker,
                            Line = lineDto.Line,
                            Expression = lineDto.Expression
                        });
                    }
                }

                cutscenes.Add(new CutsceneData
                {
                    BackgroundAsset = dto.BackgroundAsset,
                    MusicAsset = dto.MusicAsset,
                    SpriteAsset = dto.SpriteAsset,
                    Dialogue = dialogue
                });
            }

            return cutscenes;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading cutscenes for level {level}: {ex.Message}");
            return new List<CutsceneData>();
        }
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

public class CutsceneData
{
    public string BackgroundAsset { get; set; }
    public string MusicAsset { get; set; }
    public string SpriteAsset { get; set; }
    public List<DialogueLine> Dialogue { get; set; }
}

public class DialogueLine
{
    public string Speaker { get; set; }
    public string Line { get; set; }
    public string Expression { get; set; }
}
