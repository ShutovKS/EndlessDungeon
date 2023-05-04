using Data.Dynamic.PlayerData;
using Services.PersistentProgress;
using Services.Watchers.SaveLoadWatcher;
using UnityEngine;

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

        private readonly IPersistentProgressService _persistentProgressService;
        private readonly ISaveLoadInstancesWatcher _saveLoadInstancesWatcher;

        private const string SaveLoadKey = "SaveLoadKey";

        public void SaveProgress()
        {
            foreach (var progressSavable in _saveLoadInstancesWatcher.ProgressSavable)
            {
                progressSavable.UpdateProgress(_persistentProgressService.PlayerProgress);
            }

            PlayerPrefs.SetString(SaveLoadKey, JsonUtility.ToJson(_persistentProgressService.PlayerProgress));
        }

        public PlayerProgress LoadProgress()
        {
            if (!PlayerPrefs.HasKey(SaveLoadKey))
            {
                return null;
            }

            var prefs = JsonUtility.FromJson<PlayerProgress>(PlayerPrefs.GetString(SaveLoadKey));
            return prefs;
        }
    }
}