using BulletHellGame.Data;
using System.Linq;

namespace BulletHellGame.Logic.Managers
{
    public class WaveManager
    {
        private List<WaveData> _waves = new List<WaveData>();
        private EntityManager _entityManager;
        private float _elapsedTime = 0f; // Track elapsed time
        private HashSet<WaveData> _spawnedWaves = new HashSet<WaveData>(); // Track spawned waves

        public WaveManager(EntityManager entityManager)
        {
            _entityManager = entityManager;
        }

        public void AddWave(WaveData wave)
        {
            _waves.Add(wave);
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Spawn waves only once when their start time is reached
            foreach (var wave in _waves.Where(w => _elapsedTime >= w.StartTime && !_spawnedWaves.Contains(w)))
            {
                foreach (var spawnData in wave.Enemies)
                {
                    _entityManager.SpawnEnemy(spawnData.EnemyData, spawnData.SpawnPosition);
                }

                _spawnedWaves.Add(wave);
            }

            // Remove completed waves
            _waves.RemoveAll(wave => _spawnedWaves.Contains(wave));
        }
    }
}
