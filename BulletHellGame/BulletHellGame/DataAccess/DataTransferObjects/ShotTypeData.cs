namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class ShotTypeData
    {
        public string Name { get; set; }
        public ShotData UnfocusedShot { get; set; }
        public ShotData FocusedShot {  get; set; }
        public BombData UnfocusedBomb { get; set; }
        public BombData FocusedBomb { get; set; }
    }
}
