using Data.Dynamic;

namespace Services.PersistentProgress
{
    public interface IProgressSavable
    {
        public void UpdateProgress(Progress progress);
    }
}