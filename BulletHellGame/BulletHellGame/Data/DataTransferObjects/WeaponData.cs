namespace BulletHellGame.Data.DataTransferObjects
{
    public class WeaponData
    {
        public string SpriteName { get; set; }
        public BulletData bulletData {  get; set; }
        public float FireRate {  get; set; }
        public float TimeSinceLastShot { get; set; }

        public List<Vector2> FireDirections { get; set; }

        public WeaponData() { }
    }
}
