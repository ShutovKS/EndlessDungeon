using Data.Dynamic;

namespace Services.PersistentProgress
{
    public interface IProgressLoadable
    {
        public void LoadProgress(Progress progress);
    }
}