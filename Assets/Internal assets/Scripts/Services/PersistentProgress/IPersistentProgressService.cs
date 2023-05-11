using Data.Dynamic;

namespace Services.PersistentProgress
{
    public interface IPersistentProgressService
    {
        public Progress Progress { get; }
        public void SetProgress(Progress progress);
    }
}