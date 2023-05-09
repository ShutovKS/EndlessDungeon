using System.Collections.Generic;
using Services.PersistentProgress;
using UnityEngine;

namespace Services.Watchers.SaveLoadWatcher
{
    public class SaveLoadInstancesWatcher : ISaveLoadInstancesWatcher
    {
        public List<IProgressSavable> ProgressSavable { get; } = new();
        public List<IProgressLoadable> ProgressLoadable { get; } = new();

        public void RegisterProgress(params GameObject[] instances)
        {
            foreach (var instance in instances)
            {
                foreach (var progressLoader in instance.GetComponentsInChildren<IProgressLoadable>())
                {
                    Register(progressLoader);
                }
            }
        }

        public void DeleteProgress(params GameObject[] instances)
        {
            foreach (var instance in instances)
            {
                foreach (var progressLoader in instance.GetComponentsInChildren<IProgressLoadable>())
                {
                    Deletion(progressLoader);
                }
            }
        }

        public void ClearProgress()
        {
            ProgressSavable.Clear();
            ProgressLoadable.Clear();
        }

        private void Register(IProgressLoadable progressLoadable)
        {
            if (progressLoadable is IProgressSavable progressSavable)
            {
                ProgressSavable.Add(progressSavable);
            }

            ProgressLoadable.Add(progressLoadable);
        }

        private void Deletion(IProgressLoadable progressLoadable)
        {
            if (progressLoadable is IProgressSavable progressSavable)
            {
                ProgressSavable.Remove(progressSavable);
            }

            ProgressLoadable.Remove(progressLoadable);
        }
    }
}