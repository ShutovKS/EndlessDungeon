using System.Collections.Generic;
using Services.PersistentProgress;
using UnityEngine;

namespace Services.Watchers.SaveLoadWatcher
{
    public interface ISaveLoadInstancesWatcher
    {
        public List<IProgressSavable> ProgressSavable { get; }
        public List<IProgressLoadable> ProgressLoadable { get; }
        public void RegisterProgress(params GameObject[] instances);
        public void DeleteProgress(params GameObject[] instances);
        public void ClearProgress();
    }
}