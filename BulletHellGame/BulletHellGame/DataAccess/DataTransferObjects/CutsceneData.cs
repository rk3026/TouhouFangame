using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletHellGame.DataAccess.DataTransferObjects
{
    public class CutsceneData
    {
        public string BackgroundAsset { get; set; }
        public string MusicAsset { get; set; }
        public List<DialogueLine> Dialogue { get; set; }

    }

    public class DialogueLine
    {
        public string SpeakerName;
        public string Line;
        public string SpriteExpression;
    }



}
