namespace BulletHellGame.Managers
{
    public class SettingsManager
    {
        private static SettingsManager _instance;
        public static SettingsManager Instance => _instance ??= new SettingsManager();

        public float MasterVolume { get; set; } = 1.0f; // Volume range: 0.0 to 1.0
        public float MusicVolume { get; set; } = 1.0f;
        public float SFXVolume { get; set; } = 1.0f;
        public bool Debugging { get; set; } = false;
        private SettingsManager() { }
    }
}
