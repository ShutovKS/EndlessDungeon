using Data.Dynamic;
using Data.Dynamic.PlayerData;

namespace Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        PlayerProgress LoadProgress();
    }
}