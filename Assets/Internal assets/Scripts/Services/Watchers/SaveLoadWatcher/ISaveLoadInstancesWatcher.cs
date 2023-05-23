#region

using System.Collections.Generic;
using Services.Watchers.PersistentProgressWatcher;
using UnityEngine;

#endregion

namespace Services.Watchers.SaveLoadWatcher
{
    public interface ISaveLoadInstancesWatcher
    {
        public List<IProgressSavableWatcher> ProgressSavable { get; }
        public List<IProgressLoadableWatcher> ProgressLoadable { get; }
        public void RegisterProgressWatchers(params GameObject[] instances);
        public void DeleteProgressWatchers(params GameObject[] instances);
        public void ClearProgressWatchers();
    }
}
