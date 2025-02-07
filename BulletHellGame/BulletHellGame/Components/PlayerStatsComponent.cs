namespace BulletHellGame.Components
{
    public class PlayerStatsComponent : IComponent
    {
        public int Score { get; set; }
        public int CherryPoints { get; set; }
        public int CherryPlus { get; set; }
        public int Lives { get; set; }
        public int Bombs { get; set; }
        public float Power { get; private set; }
        public float MaxPower { get; }

        public PlayerStatsComponent(int initialLives, int initialBombs, float maxPower = 4.00f)
        {
            Score = 0;
            CherryPoints = 0;
            CherryPlus = 0;
            Lives = initialLives;
            Bombs = initialBombs;
            MaxPower = maxPower;
            Power = 0;
        }

        // Method to increase power while enforcing max limit
        public void AddPower(float amount)
        {
            Power = Math.Min(MaxPower, Power + amount);
        }
    }
}
