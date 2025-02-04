namespace BulletHellGame.Data.DataTransferObjects
{
    public class PlayerData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpriteName { get; set; }
        public int Health { get; set; }
        public float MovementSpeed { get; set; }
        public float FocusedSpeed { get; set; }
        public int InitialBombs { get; set; }
        public string BombCherryLoss { get; set; }
        public List<string> SpecialAbility { get; set; } = new();
        public List<ShotType> ShotTypes { get; set; } = new();
        public Dictionary<Vector2, WeaponData> WeaponsAndOffsets { get; set; } = new();
        public List<Ability> Abilities { get; set; } = new();
    }

    public class ShotType
    {
        public string Name { get; set; }
        public string UnfocusedShot { get; set; }
        public string FocusedShot { get; set; }
        public string UnfocusedBomb { get; set; }
        public string FocusedBomb { get; set; }
    }

    public class Ability
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Cooldown { get; set; }
        public string Effect { get; set; }
        public int Duration { get; set; }
        public int AreaOfEffect { get; set; }
    }
}
