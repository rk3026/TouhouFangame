using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public static class CutsceneDataLoader
{
    private static readonly string CUTSCENE_PATH = "Content/Data/Cutscenes";

    public static List<CutsceneDto> LoadCutscenesForLevel(int level)
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

            List<CutsceneDto> cutsceneDtos = JsonSerializer.Deserialize<List<CutsceneDto>>(json, options);
            return cutsceneDtos ?? new List<CutsceneDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading cutscenes for level {level}: {ex.Message}");
            return new List<CutsceneDto>();
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

