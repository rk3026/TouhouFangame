namespace BulletHellGame.Data.DataTransferObjects
{
    public class BossData : EnemyData
    {
        public Dictionary<int, BossPhaseData> Phases { get; set; } // int = health threshold where it reaches that phase.
    }
}
