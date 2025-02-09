namespace BulletHellGame.Data.DataTransferObjects
{
    public class CharacterData
    {
        public string Id { get; set; }
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
        public Dictionary<int, PlayerShootingData> PowerLevels { get; set; } = new();

        public List<string> SpecialAbilities { get; set; } = new();
        public List<ShotTypeDescription> ShotTypeDescriptions { get; set; } = new();
    }

    public class ShotTypeDescription
    {
        public string Name { get; set; }
        public string UnfocusedShot { get; set; }
        public string FocusedShot { get; set; }
        public string UnfocusedBomb { get; set; }
        public string FocusedBomb { get; set; }
    }
}
