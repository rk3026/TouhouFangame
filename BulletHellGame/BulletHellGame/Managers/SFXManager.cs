using Microsoft.Xna.Framework.Audio;

namespace BulletHellGame.Managers
{
    public class SFXManager
    {
        private static SFXManager _instance;
        public static SFXManager Instance => _instance ??= new SFXManager();
        public Dictionary<string, SoundEffect> SoundEffects { get; private set; }

        public SFXManager() { }

    }
}
