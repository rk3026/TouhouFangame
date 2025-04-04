using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BulletHellGame.DataAccess.DataTransferObjects;

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
                            SpeakerName = lineDto.Speaker,
                            Line = lineDto.Line,
                            SpriteExpression = lineDto.Expression
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

    public static CutsceneData LoadCutsceneData(string cutsceneName)
    {
        try
        {
            string cutsceneFilePath = Path.Combine(CUTSCENE_PATH, $"{cutsceneName}.json");

            if (!File.Exists(cutsceneFilePath))
                throw new FileNotFoundException($"Cutscene file not found: {cutsceneFilePath}");

            string json = File.ReadAllText(cutsceneFilePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            CutsceneDto dto = JsonSerializer.Deserialize<CutsceneDto>(json, options);

            var dialogue = new List<DialogueLine>();
            if (dto.Dialogue != null)
            {
                foreach (var lineDto in dto.Dialogue)
                {
                    dialogue.Add(new DialogueLine
                    {
                        SpeakerName = lineDto.Speaker,
                        Line = lineDto.Line,
                        SpriteExpression = lineDto.Expression
                    });
                }
            }

            return new CutsceneData
            {
                BackgroundAsset = dto.BackgroundAsset,
                MusicAsset = dto.MusicAsset,
                SpriteAsset = dto.SpriteAsset,
                Dialogue = dialogue
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading cutscene {cutsceneName}: {ex.Message}");
            return null;
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

