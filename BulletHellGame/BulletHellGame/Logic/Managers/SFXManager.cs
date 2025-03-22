﻿using Microsoft.Xna.Framework.Audio;
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

        public void PlaySound(string soundName, bool loop = false, float pitchVariance = 0.1f, float volumeVariance = 0.1f, float panVariance = 0.1f)
        {
            if (_soundEffects.TryGetValue(soundName, out SoundEffect soundEffect))
            {
                var instance = soundEffect.CreateInstance();
                instance.IsLooped = loop;

                Random random = new Random();
                float masterVolume = SettingsManager.Instance.MasterVolume;
                float sfxVolume = SettingsManager.Instance.SFXVolume;

                // Randomize pitch, volume, and pan
                instance.Pitch = MathHelper.Clamp((float)(random.NextDouble() * 2 - 1) * pitchVariance, -1f, 1f);
                instance.Volume = MathHelper.Clamp(masterVolume * sfxVolume * (1f + (float)(random.NextDouble() * 2 - 1) * volumeVariance), 0f, 1f) * GLOBAL_SFX_SCALE; // also reduce volume because SFX are loud
                instance.Pan = MathHelper.Clamp((float)(random.NextDouble() * 2 - 1) * panVariance, -1f, 1f);

                instance.Play();
            }
        }

        public void StopSound(string soundName)
        {
            if (_soundInstances.TryGetValue(soundName, out SoundEffectInstance instance))
            {
                instance.Stop();
            }
        }
    }
}
