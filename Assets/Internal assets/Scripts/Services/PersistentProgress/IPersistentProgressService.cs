using Data.Dynamic;
using Data.Dynamic.PlayerData;

namespace Services.PersistentProgress
{
    public interface IPersistentProgressService
    {
        public PlayerProgress PlayerProgress { get; }
        public void SetProgress(PlayerProgress playerProgress);
    }
}