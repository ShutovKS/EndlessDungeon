#region

using Data.Dynamic;

#endregion

namespace Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public Progress Progress { get; private set; }

        public void SetProgress(Progress progress)
        {
            Progress = progress;
        }
    }
}
