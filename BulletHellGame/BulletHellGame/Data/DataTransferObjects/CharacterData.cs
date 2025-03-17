namespace BulletHellGame.Data.DataTransferObjects
{
    public class CharacterData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpriteName { get; set; }
        public int Health { get; set; }
        public Vector2 HitboxSize { get; set; }
        public float MovementSpeed { get; set; }
        public float FocusedSpeed { get; set; }
        public int InitialLives { get; set; }
        public int InitialBombs { get; set; }
        public string BombCherryLoss { get; set; }
        public float DeathbombWindow { get; set; } = 0.33f;
        public List<string> SpecialAbilities { get; set; } = new();
        public List<ShotTypeData> ShotTypes { get; set; }
        public Dictionary<int, PowerLevelData> UnfocusedPowerLevels { get; set; } = new();
        public Dictionary<int, PowerLevelData> FocusedPowerLevels { get; set; } = new();
    }
}
