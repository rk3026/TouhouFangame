using BulletHellGame.Data.DataTransferObjects;

namespace BulletHellGame.Logic.Components
{
    public class BossPhaseComponent : IComponent
    {
        public int CurrentPhase { get; private set; }
        public List<EnemyData> Phases { get; private set; }

        public BossPhaseComponent(List<EnemyData> phases)
        {
            Phases = phases;
            CurrentPhase = 0;
        }

        public EnemyData GetCurrentPhaseData()
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
