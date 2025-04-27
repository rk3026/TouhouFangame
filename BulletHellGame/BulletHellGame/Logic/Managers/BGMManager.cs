using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace BulletHellGame.Logic.Managers
{
    public class BGMManager
    {
        private static readonly float GLOBAL_BGM_SCALE = 0.1f;

        private static BGMManager _instance;
        public static BGMManager Instance => _instance ??= new BGMManager();

        private Song _currentBGM;
        private string _currentTrack;

        private BGMManager()
        {
            // Subscribe to settings changes
            SettingsManager.Instance.OnVolumeChanged += () => SetVolume(SettingsManager.Instance.MusicVolume);
        }

        public void PlayBGM(ContentManager contentManager, string trackName, bool loop = true)
        {
            string trackPath = Path.Combine("Music", trackName);

            if (_currentTrack == trackPath)
            {
                // Track is already playing
                return;
            }

            // Stop the current track if there is one
            StopBGM();

            // Load and play the new track
            _currentBGM = contentManager.Load<Song>(trackPath);
            SetVolume(SettingsManager.Instance.MusicVolume);
            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Play(_currentBGM);
            _currentTrack = trackPath;
        }

        public void StopBGM()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
                _currentTrack = null;
            }
        }
        public void PauseBGM()
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Pause();
            }
        }
        public void ResumeBGM()
        {
            if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
        }

        private void SetVolume(float volume)
        {
            float masterVolume = SettingsManager.Instance.MasterVolume;
            MediaPlayer.Volume = MathHelper.Clamp(masterVolume * volume, 0f, 1f) * GLOBAL_BGM_SCALE;
        }
    }
}
