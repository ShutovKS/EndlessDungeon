#region

using Data.Dynamic;

#endregion

namespace Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        Progress LoadProgress();
        void ClearProgress();
        bool IsInStockSave();
    }
}
