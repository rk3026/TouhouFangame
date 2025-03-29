using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Entities;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BulletHellGame.Logic.Managers
{
    public class LevelManager
    {
        private readonly EntityManager _entityManager;
        private readonly WaveManager _waveManager;
        private readonly Rectangle _playableArea;

        private Dictionary<int, LevelData> _levels;
        private int _currentLevel = 1;
        private int _totalLevels = 1;

        private bool _subBossSpawned = false;
        private bool _bossSpawned = false;
        private bool _levelComplete = false;

        public bool WaveJustCompleted => _waveManager.WaveJustCompleted;
        public bool SubBossSpawned => _subBossSpawned;
        public bool BossSpawned => _bossSpawned;
        public bool LevelComplete => _levelComplete;

        public LevelManager(EntityManager entityManager, Rectangle playableArea)
        {
            _entityManager = entityManager;
            _playableArea = playableArea;
            _waveManager = new WaveManager(_entityManager, playableArea);

            LoadLevelData();
        }

        private void LoadLevelData()
        {
            _levels = new Dictionary<int, LevelData>();

            for (int level = 1; level <= _totalLevels; level++)
            {
                var levelData = EntityDataGenerator.GenerateLevelData(_playableArea);
                _levels[level] = levelData;
            }
        }

        public void StartLevel(int level)
        {
            if (!_levels.ContainsKey(level)) return;

            _currentLevel = level;
            _subBossSpawned = false;
            _bossSpawned = false;
            _levelComplete = false;

            _waveManager.ClearWaves();

            foreach (var wave in _levels[level].Waves)
            {
                _waveManager.AddWave(wave);
            }
        }

        public void Update(GameTime gameTime)
        {
            _waveManager.Update(gameTime);

            if (_waveManager.AreAllWavesComplete() && !_subBossSpawned)
            {
                SpawnSubBoss();
            }

            if (_subBossSpawned && !_bossSpawned && _entityManager.GetEntityCount(EntityType.Boss) == 0)
            {
                SpawnBoss();
            }

            if (_bossSpawned && _entityManager.GetEntityCount(EntityType.Boss) == 0)
            {
                _levelComplete = true;
            }
        }

        public bool StartNextLevel()
        {
            if (_currentLevel < _totalLevels)
            {
                StartLevel(_currentLevel + 1);
                return true;
            }
            else
            {
                return false; // Game finished
            }
        }

        public void SpawnSubBoss()
        {
            if (_subBossSpawned || !_levels.ContainsKey(_currentLevel))
                return;

            var level = _levels[_currentLevel];
            if (level.SubBoss == null) return;

            Vector2 spawnPos = new Vector2(
                _entityManager.Bounds.Width / 2,
                _entityManager.Bounds.Top + 50
            );

            _entityManager.SpawnBoss(level.SubBoss, spawnPos);
            _subBossSpawned = true;
        }

        public void SpawnBoss()
        {
            if (_bossSpawned || !_levels.ContainsKey(_currentLevel))
                return;

            var level = _levels[_currentLevel];
            if (level.Boss == null) return;

            Vector2 spawnPos = new Vector2(
                _entityManager.Bounds.Width / 2,
                _entityManager.Bounds.Top + 50
            );

            _entityManager.SpawnBoss(level.Boss, spawnPos);
            _bossSpawned = true;
        }

        public bool AreAllWavesComplete() => _waveManager.AreAllWavesComplete();
    }
}
