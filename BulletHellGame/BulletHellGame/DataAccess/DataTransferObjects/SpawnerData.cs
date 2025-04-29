using BulletHellGame.Logic.Entities;

namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class SpawnerData
    {
        public List<WeaponData> Weapons { get; set; } = new List<WeaponData>();
        public Entity Owner { get; set; }
        public string SpriteName { get; set; }
        public Vector2 Offset { get; set; }
        public string MovementPattern { get; set; } = string.Empty;

        public SpawnerData() { }

        public SpawnerData(List<WeaponData> weapons, Entity owner, string spriteName, string movementPattern)
        {
            Weapons = weapons;
            Owner = owner;
            SpriteName = spriteName;
            Offset = Vector2.Zero;
            MovementPattern = movementPattern;
        }
    }
}
