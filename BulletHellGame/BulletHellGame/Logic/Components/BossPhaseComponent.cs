using BulletHellGame.DataAccess.DataTransferObjects;

namespace BulletHellGame.Logic.Components
{
    public class BossPhaseComponent : IComponent
    {
        public int CurrentPhase { get; private set; }
        public List<GruntData> Phases { get; private set; }

        public BossPhaseComponent(List<GruntData> phases)
        {
            Phases = phases;
            CurrentPhase = 0;
        }

        public GruntData GetCurrentPhaseData()
        {
            return Phases[CurrentPhase];
        }

        public bool AdvancePhase()
        {
            if (CurrentPhase + 1 < Phases.Count)
            {
                CurrentPhase++;
                return true;
            }
            return false;
        }
    }
}
