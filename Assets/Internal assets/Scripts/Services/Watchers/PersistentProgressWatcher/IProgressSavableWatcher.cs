#region

using Data.Dynamic;

#endregion

namespace Services.Watchers.PersistentProgressWatcher
{
    public interface IProgressSavableWatcher
    {
        public void UpdateProgress(Progress progress);
    }
}
