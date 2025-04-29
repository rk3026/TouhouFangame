namespace BulletHellGame.Logic.Components
{
    public class PlayerStatsComponent : IComponent
    {
        public int Score { get; set; }
        public int CherryPoints { get; set; }
        public int CherryPlus { get; set; }
        public int Lives { get; set; }
        public float Power { get; set; }
        public int CurrentPowerLevel { get; set; }

        public PlayerStatsComponent(int initialLives, int initialBombs)
        {
            Score = 0;
            CherryPoints = 0;
            CherryPlus = 0;
            Lives = initialLives;
            Power = 0;
        }
    }
}
