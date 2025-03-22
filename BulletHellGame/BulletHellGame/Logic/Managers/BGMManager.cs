using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace BulletHellGame.Logic.Managers
{
    public class BGMManager
    {
        private static BGMManager _instance;
        public static BGMManager Instance => _instance ??= new BGMManager();

        private Song _currentBGM;
        private string _currentTrack;

        private BGMManager() { }

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

        public void SetVolume(float volume)
        {
            float masterVolume = SettingsManager.Instance.MasterVolume;
            MediaPlayer.Volume = MathHelper.Clamp(masterVolume * volume, 0f, 1f) * 0.1f; // Songs are FUCKING loud so reduce volume
        }

        public void UpdateVolumeFromSettings()
        {
            SetVolume(SettingsManager.Instance.MusicVolume);
        }
    }
}
