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
        public List<string> SpecialAbility { get; set; }
        public List<ShotType> ShotTypes { get; set; }
        public List<Weapon> Weapons { get; set; }
        public List<Ability> Abilities { get; set; }
        public Audio Audio { get; set; }
    }

    public class ShotType
    {
        public string Name { get; set; }
        public string UnfocusedShot { get; set; }
        public string FocusedShot { get; set; }
        public string UnfocusedBomb { get; set; }
        public string FocusedBomb { get; set; }
    }

    public class Weapon
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Damage { get; set; }
        public double FireRate { get; set; }
        public int Speed { get; set; }
        public int Range { get; set; }
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

    public class Audio
    {
        public string AttackSound { get; set; }
        public string AbilitySound { get; set; }
    }
}
