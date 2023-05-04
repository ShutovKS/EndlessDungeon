using Data.Dynamic;
using Data.Dynamic.PlayerData;

namespace Services.PersistentProgress
{
    public interface IProgressLoadable
    {
        public void LoadProgress(PlayerProgress playerProgress);
    }
}