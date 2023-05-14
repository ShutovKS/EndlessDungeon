using Data.Dynamic;
using Data.Dynamic.Loot;

namespace Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        Progress LoadProgress();
        void ClearProgress();
    }
}