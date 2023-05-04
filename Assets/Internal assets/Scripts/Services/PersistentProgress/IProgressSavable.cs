using Data.Dynamic;
using Data.Dynamic.PlayerData;

namespace Services.PersistentProgress
{
    public interface IProgressSavable
    {
        public void UpdateProgress(PlayerProgress playerProgress);
    }
}