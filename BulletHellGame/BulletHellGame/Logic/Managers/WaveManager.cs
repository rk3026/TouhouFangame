using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;

namespace BulletHellGame.Logic.Managers
{
    public class WaveManager
    {
        private readonly EntityManager _entityManager;
        private readonly Rectangle _playableArea;

        private readonly Queue<WaveData> _waveQueue = new();
        private WaveState _currentWave = null;
        private float _globalTime = 0f;
        private List<Entity> _spawnedEnemies = new();

        public bool WaveJustCompleted { get; private set; } = false;

        public event Action BossSpawned;
        public event Action BossDefeated;

        public WaveManager(EntityManager entityManager, Rectangle playableArea)
        {
            _entityManager = entityManager;
            _playableArea = playableArea;
        }

        public void AddWave(WaveData wave)
        {
            _waveQueue.Enqueue(wave);
        }

        public void ClearWaves()
        {
            _waveQueue.Clear();
            _currentWave = null;
            _globalTime = 0f;
            WaveJustCompleted = false;
        }

        public void Update(GameTime gameTime)
        {
            _globalTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            WaveJustCompleted = false;

            // Start next wave if needed
            if (_currentWave == null && _waveQueue.Count > 0)
            {
                var nextWave = _waveQueue.Dequeue();
                _currentWave = new WaveState(nextWave);

                // If the wave type is Boss, invoke the BossSpawned event when switching to a boss wave
                if (_currentWave.Wave.WaveType == WaveType.Boss || _currentWave.Wave.WaveType == WaveType.SubBoss)
                {
                   // TODO: Check back here later
                    BossSpawned?.Invoke();
                }
            }

            if (_currentWave != null)
            {
                var wave = _currentWave.Wave;
                _currentWave.ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Spawn enemies
                foreach (var enemySpawn in wave.Enemies)
                {
                    if (!_currentWave.Spawned.Contains(enemySpawn) &&
                        _currentWave.ElapsedTime >= enemySpawn.SpawnTime)
                    {
                        Entity entity;
                        if (enemySpawn.EnemyData.GetType() == typeof(BossData))
                        {
                            entity = _entityManager.SpawnBoss((BossData)enemySpawn.EnemyData, enemySpawn.SpawnPosition);
                        }
                        else
                        {
                            entity = _entityManager.SpawnEnemy((GruntData)enemySpawn.EnemyData, enemySpawn.SpawnPosition);
                        }
                        _currentWave.Spawned.Add(enemySpawn);
                        _spawnedEnemies.Add(entity);
                    }
                }

                // Completion logic
                bool allEnemiesSpawned = _currentWave.Spawned.Count == wave.Enemies.Count;
                bool allEnemiesDefeated = _entityManager.GetEntityCount(EntityType.Enemy) == 0 && _entityManager.GetEntityCount(EntityType.Boss) == 0;
                bool durationExpired = _currentWave.ElapsedTime >= wave.Duration;

                if (allEnemiesSpawned && allEnemiesDefeated || durationExpired)
                {
                    if (_currentWave.Wave.WaveType == WaveType.Boss || _currentWave.Wave.WaveType == WaveType.SubBoss)
                    {
                        BossDefeated?.Invoke();
                    }
                    _currentWave = null;
                    WaveJustCompleted = true;

                    // Remove all spawned enemies if the wave is complete
                    _spawnedEnemies.ForEach(e => _entityManager.QueueEntityForRemoval(e));
                }
            }
        }


        public bool AreAllWavesComplete()
        {
            return _currentWave == null && _waveQueue.Count == 0;
        }

        public WaveType GetCurrentWaveType()
        {
            return _currentWave?.Wave.WaveType ?? WaveType.Normal;
        }

        internal float GetCurrentWaveTimeLeft()
        {
            if (_currentWave == null) return 0f;
            return _currentWave.Wave.Duration - _currentWave.ElapsedTime;
        }

        private class WaveState
        {
            public WaveData Wave { get; }
            public float ElapsedTime { get; set; } = 0f;
            public HashSet<EnemySpawnData> Spawned { get; } = new();

            public WaveState(WaveData wave)
            {
                Wave = wave;
            }
        }
    }
}
