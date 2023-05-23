#region

using Data.Dynamic;

#endregion

namespace Services.Watchers.PersistentProgressWatcher
{
    public interface IProgressLoadableWatcher
    {
        public void LoadProgress(Progress progress);
    }
}
