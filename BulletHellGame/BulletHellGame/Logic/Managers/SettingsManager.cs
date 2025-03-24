namespace BulletHellGame.Logic.Managers
{
    public class SettingsManager
    {
        private static SettingsManager _instance;
        public static SettingsManager Instance => _instance ??= new SettingsManager();

        public event Action OnVolumeChanged;
        public event Action OnDebuggingChanged;

        private float _masterVolume = 1.0f;
        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                if (_masterVolume != value)
                {
                    _masterVolume = MathHelper.Clamp(value, 0f, 1f);
                    OnVolumeChanged?.Invoke();
                }
            }
        }

        private float _musicVolume = 1.0f;
        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                if (_musicVolume != value)
                {
                    _musicVolume = MathHelper.Clamp(value, 0f, 1f);
                    OnVolumeChanged?.Invoke();
                }
            }
        }

        private float _sfxVolume = 1.0f;
        public float SFXVolume
        {
            get => _sfxVolume;
            set
            {
                if (_sfxVolume != value)
                {
                    _sfxVolume = MathHelper.Clamp(value, 0f, 1f);
                    OnVolumeChanged?.Invoke();
                }
            }
        }

        private bool _debugging = false;
        public bool Debugging
        {
            get => _debugging;
            set
            {
                if (_debugging != value)
                {
                    _debugging = value;
                    OnDebuggingChanged?.Invoke();
                }
            }
        }

        private SettingsManager() { }
    }
}
