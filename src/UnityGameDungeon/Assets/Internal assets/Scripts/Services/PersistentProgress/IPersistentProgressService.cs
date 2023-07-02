#region

using Data.Dynamic;

#endregion

namespace Services.PersistentProgress
{
    public interface IPersistentProgressService
    {
        Progress Progress { get; }
        void SetProgress(Progress progress);
    }
}
