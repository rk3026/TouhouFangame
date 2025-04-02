using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace BulletHellGame.Logic.Managers
{
    public class SFXManager
    {
        private static readonly float GLOBAL_SFX_SCALE = 0.1f;

        private static SFXManager _instance;
        public static SFXManager Instance => _instance ??= new SFXManager();

        private readonly Dictionary<string, SoundEffect> _soundEffects;
        private readonly Dictionary<string, SoundEffectInstance> _soundInstances;

        private SFXManager()
        {
            _soundEffects = new Dictionary<string, SoundEffect>();
            _soundInstances = new Dictionary<string, SoundEffectInstance>();

            // Subscribe to volume changes
            SettingsManager.Instance.OnVolumeChanged += UpdateAllVolumes;
        }

        public void LoadSound(ContentManager contentManager, string soundName, string soundPath)
        {
            if (!_soundEffects.ContainsKey(soundName))
            {
                var soundEffect = contentManager.Load<SoundEffect>(soundPath);
                _soundEffects[soundName] = soundEffect;
                _soundInstances[soundName] = soundEffect.CreateInstance();
            }
        }

        public void PlaySound(string soundName, bool loop = false, bool varySound = true)
        {
            varySound = false; // Disable sound variance for now, there is a bug where it somehow changes the pitch/variance of the bgmusic too, wtfrick
            if (_soundEffects.TryGetValue(soundName, out SoundEffect soundEffect))
            {
                var instance = soundEffect.CreateInstance();
                instance.IsLooped = loop;

                Random random = new Random();
                float masterVolume = SettingsManager.Instance.MasterVolume;
                float sfxVolume = SettingsManager.Instance.SFXVolume;

                // Default variance values
                float pitchVariance = varySound ? 0.1f : 0f;
                float volumeVariance = varySound ? 0.1f : 0f;
                float panVariance = varySound ? 0.1f : 0f;

                // Apply variations only if enabled
                instance.Pitch = MathHelper.Clamp((float)(random.NextDouble() * 2 - 1) * pitchVariance, -1f, 1f);
                instance.Volume = MathHelper.Clamp(masterVolume * sfxVolume * (1f + (float)(random.NextDouble() * 2 - 1) * volumeVariance), 0f, 1f) * GLOBAL_SFX_SCALE;
                instance.Pan = MathHelper.Clamp((float)(random.NextDouble() * 2 - 1) * panVariance, -1f, 1f);

                instance.Play();
                _soundInstances[soundName] = instance; // Track active sounds for volume updates
            }
        }


        public void StopSound(string soundName)
        {
            if (_soundInstances.TryGetValue(soundName, out SoundEffectInstance instance))
            {
                instance.Stop();
                _soundInstances.Remove(soundName);
            }
        }

        private void UpdateAllVolumes()
        {
            float masterVolume = SettingsManager.Instance.MasterVolume;
            float sfxVolume = SettingsManager.Instance.SFXVolume;

            foreach (var instance in _soundInstances.Values)
            {
                instance.Volume = MathHelper.Clamp(masterVolume * sfxVolume, 0f, 1f) * GLOBAL_SFX_SCALE;
            }
        }
    }
}
