namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class CutsceneData
    {
        public string BackgroundAsset { get; set; }
        public string MusicAsset { get; set; }
        public string SpriteAsset { get; set; }
        public List<DialogueLine> Dialogue { get; set; }
    }

    public class DialogueLine
    {
        public string SpeakerName { get; set; }
        public string Line { get; set; }
        public string SpriteExpression { get; set; }
        public float TypeDelay { get; set; } = 0.03f;
        public bool FlashAfterLine { get; set; } = false;
    }
}
