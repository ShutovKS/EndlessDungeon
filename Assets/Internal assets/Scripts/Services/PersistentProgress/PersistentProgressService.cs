using Data.Dynamic.PlayerData;

namespace Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress PlayerProgress { get; private set; }

        public void SetProgress(PlayerProgress playerProgress)
        {
            PlayerProgress = playerProgress;
        }
    }
}