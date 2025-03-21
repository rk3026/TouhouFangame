using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BulletHellGame.Logic.Managers
{
    public class WaveManager
    {
        private readonly EntityManager _entityManager;
        private readonly Rectangle _playableArea;

        private readonly Queue<WaveData> _waveQueue = new();
        private WaveState _currentWave = null;
        private float _globalTime = 0f;

        public bool WaveJustCompleted { get; private set; } = false;

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
            }

            if (_currentWave != null)
            {
                var wave = _currentWave.Wave;
                _currentWave.ElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                foreach (var enemySpawn in wave.Enemies)
                {
                    if (!_currentWave.Spawned.Contains(enemySpawn) &&
                        _currentWave.ElapsedTime >= enemySpawn.SpawnTime)
                    {
                        _entityManager.SpawnEnemy(enemySpawn.EnemyData, enemySpawn.SpawnPosition);
                        _currentWave.Spawned.Add(enemySpawn);
                    }
                }

                // Completion logic
                bool allEnemiesSpawned = _currentWave.Spawned.Count == wave.Enemies.Count;
                bool allEnemiesDefeated = _entityManager.GetEntityCount(EntityType.Enemy) == 0;
                bool durationExpired = _currentWave.ElapsedTime >= wave.Duration;

                if ((allEnemiesSpawned && allEnemiesDefeated) || durationExpired)
                {
                    _currentWave = null;
                    WaveJustCompleted = true;
                }
            }
        }

        public bool AreAllWavesComplete()
        {
            return _currentWave == null && _waveQueue.Count == 0;
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
