#region

using Data.Dynamic;
using Services.PersistentProgress;
using Services.Watchers.SaveLoadWatcher;
using UnityEngine;

#endregion

namespace Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        public SaveLoadService(IPersistentProgressService persistentProgressService,
            ISaveLoadInstancesWatcher saveLoadInstancesWatcher)
        {
            _persistentProgressService = persistentProgressService;
            _saveLoadInstancesWatcher = saveLoadInstancesWatcher;
        }

        private const string SAVE_LOAD_KEY = "SaveLoadKey";

        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;

        public void SaveProgress()
        {
            foreach (var progressSavable in _saveLoadInstancesWatcher.ProgressSavable)
            {
                progressSavable.UpdateProgress(_persistentProgressService.Progress);
            }

            PlayerPrefs.SetString(SAVE_LOAD_KEY, JsonUtility.ToJson(_persistentProgressService.Progress));
        }

        public Progress LoadProgress()
        {
            if (!IsInStockSave())
                return null;

            var prefs = JsonUtility.FromJson<Progress>(PlayerPrefs.GetString(SAVE_LOAD_KEY));
            return prefs;
        }

        public void ClearProgress()
        {
            if (IsInStockSave())
                PlayerPrefs.DeleteKey(SAVE_LOAD_KEY);
        }

        public bool IsInStockSave()
        {
            return PlayerPrefs.HasKey(SAVE_LOAD_KEY);
        }
    }
}
