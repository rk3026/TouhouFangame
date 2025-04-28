using BulletHellGame.DataAccess.DataLoaders;
using BulletHellGame.DataAccess.DataTransferObjects;
using BulletHellGame.Logic.Managers;
using BulletHellGame.Logic.Utilities.EntityDataGenerator;
using BulletHellGame.Logic.Utilities.EntityDataGenerator.EntityDataGenerators;
using BulletHellGame.Presentation.Scenes;

public class LevelManager
{
    private readonly EntityManager _entityManager;
    private readonly WaveManager _waveManager;
    private readonly Rectangle _playableArea;

    private Dictionary<int, LevelData> _levels;
    private int _currentLevel = 1;
    private int _totalLevels = 1;

    public bool WaveJustCompleted => _waveManager.WaveJustCompleted;
    public bool BossSpawned { get; }

    public event Action OnBossSpawned;
    public event Action OnBossDefeated;

    public LevelManager(EntityManager entityManager, Rectangle playableArea)
    {
        _entityManager = entityManager;
        _playableArea = playableArea;
        _waveManager = new WaveManager(_entityManager, playableArea);
        _waveManager.BossSpawned += () => OnBossSpawned?.Invoke();
        _waveManager.BossDefeated += () => OnBossDefeated?.Invoke();

        LoadLevelData();
    }

    private void LoadLevelData()
    {
        _levels = LevelDataLoader.LoadLevelData(_playableArea);
        _totalLevels = _levels.Count;

        /*_levels = new Dictionary<int, LevelData>();

        for (int level = 1; level <= _totalLevels; level++)
        {
            var levelData = EntityDataGenerator.GenerateLevelData(_playableArea);
            _levels[level] = levelData;
        }*/
    }

    public void StartLevel(int level)
    {
        if (!_levels.ContainsKey(level)) return;

        _currentLevel = level;
        _waveManager.ClearWaves();

        foreach (var wave in _levels[level].Waves)
        {
            _waveManager.AddWave(wave);
        }
        
    }

    public void Update(GameTime gameTime)
    {
        _waveManager.Update(gameTime);
    }

    public bool StartNextLevel()
    {
        if (_currentLevel < _totalLevels)
        {
            StartLevel(_currentLevel + 1);
            return true;
        }
        return false;
    }

    public bool LevelComplete() => _waveManager.AreAllWavesComplete();
    public WaveType GetCurrentWaveType() => _waveManager.GetCurrentWaveType();
    public float GetCurrentWaveTimeLeft() => _waveManager.GetCurrentWaveTimeLeft();
}
