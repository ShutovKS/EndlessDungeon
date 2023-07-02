#region

using System.Collections.Generic;
using Services.Watchers.PersistentProgressWatcher;
using UnityEngine;

#endregion

namespace Services.Watchers.SaveLoadWatcher
{
    public class SaveLoadInstancesWatcher : ISaveLoadInstancesWatcher
    {
        public List<IProgressSavableWatcher> ProgressSavable { get; } = new List<IProgressSavableWatcher>();
        public List<IProgressLoadableWatcher> ProgressLoadable { get; } = new List<IProgressLoadableWatcher>();

        public void RegisterProgressWatchers(params GameObject[] instances)
        {
            foreach (var instance in instances)
            {
                foreach (var progressLoadable in instance.GetComponentsInChildren<IProgressLoadableWatcher>())
                {
                    if (progressLoadable is IProgressSavableWatcher progressSavable)
                    {
                        ProgressSavable.Add(progressSavable);
                    }

                    ProgressLoadable.Add(progressLoadable);
                }
            }
        }

        public void DeleteProgressWatchers(params GameObject[] instances)
        {
            foreach (var instance in instances)
            {
                foreach (var progressLoadable in instance.GetComponentsInChildren<IProgressLoadableWatcher>())
                {
                    if (progressLoadable is IProgressSavableWatcher progressSavable)
                    {
                        ProgressSavable.Remove(progressSavable);
                    }

                    ProgressLoadable.Remove(progressLoadable);
                }
            }
        }

        public void ClearProgressWatchers()
        {
            ProgressSavable.Clear();
            ProgressLoadable.Clear();
        }
    }
}
