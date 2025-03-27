public class CutsceneManager
{
    private static CutsceneManager _instance;
    public static CutsceneManager Instance => _instance ??= new CutsceneManager();

    // ✅ Corrected: Use CutsceneData, not CutsceneDto
    private Dictionary<int, List<CutsceneData>> cutscenesPerLevel = new();

    public void LoadCutscenesForLevel(int level)
    {
        if (!cutscenesPerLevel.ContainsKey(level))
        {
            var cutscenes = CutsceneDataLoader.LoadCutscenesForLevel(level);
            cutscenesPerLevel[level] = cutscenes;
        }
    }

    public List<CutsceneData> GetCutscenesForLevel(int level)
    {
        return cutscenesPerLevel.TryGetValue(level, out var cutscenes) ? cutscenes : new List<CutsceneData>();
    }
}