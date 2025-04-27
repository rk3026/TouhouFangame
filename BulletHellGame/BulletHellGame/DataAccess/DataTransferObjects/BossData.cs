namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class BossData : IEnemyData
    {
        public string Name
        {
            get => CurrentPhase?.Name ?? "Unnamed Boss";
            set { if (CurrentPhase != null) CurrentPhase.Name = value; }
        }

        public string SpriteName
        {
            get => CurrentPhase?.SpriteName;
            set { if (CurrentPhase != null) CurrentPhase.SpriteName = value; }
        }

        public int Health
        {
            get => CurrentPhase?.Health ?? 0;
            set { if (CurrentPhase != null) CurrentPhase.Health = value; }
        }

        public string MovementPattern
        {
            get => CurrentPhase?.MovementPattern;
            set { if (CurrentPhase != null) CurrentPhase.MovementPattern = value; }
        }

        public ShootingPatternData BulletPattern
        {
            get => CurrentPhase?.BulletPattern;
            set { if (CurrentPhase != null) CurrentPhase.BulletPattern = value; }
        }

        public List<WeaponData> Weapons
        {
            get => CurrentPhase?.Weapons;
            set { if (CurrentPhase != null) CurrentPhase.Weapons = value; }
        }

        public List<CollectibleData> Loot
        {
            get => CurrentPhase?.Loot;
            set { if (CurrentPhase != null) CurrentPhase.Loot = value; }
        }

        public List<GruntData> Phases { get; set; } = new();
        public int CurrentPhaseIndex { get; set; } = 0;

        private GruntData CurrentPhase =>
            (Phases.Count > 0 && CurrentPhaseIndex < Phases.Count) ? Phases[CurrentPhaseIndex] : null;
    }
}
