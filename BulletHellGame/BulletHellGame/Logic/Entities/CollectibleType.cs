namespace BulletHellGame.Logic.Entities
{
    public enum CollectibleType
    {
        PowerUp,    // Increases Shot Power after certain threshold
        PointItem,  // Gives extra lives if collecting enough, higher up on the screen rewards more points
        OneUp,      // Gives player one life
        Bomb,       // Gives an extra bomb
        StarItem,   // Rewards some cherry and normal points
        CherryItem  // Adds to Cherry and Cherry+ points
    }
}
