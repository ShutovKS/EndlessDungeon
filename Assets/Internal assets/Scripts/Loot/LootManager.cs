#region

using System;
using Data.Dynamic;
using Services.Watchers.PersistentProgressWatcher;
using UnityEngine;

#endregion

namespace Loot
{
    public class LootManager : MonoBehaviour, IProgressLoadableWatcher, IProgressSavableWatcher
    {
        private Action<int> _onAmountChanged;
        public int SoulsOfTheDungeon { get; private set; }

        public void LoadProgress(Progress progress)
        {
            SoulsOfTheDungeon = progress.lootData.loot;
            _onAmountChanged?.Invoke(SoulsOfTheDungeon);
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lootData.loot = SoulsOfTheDungeon;
        }

        public void RegisterWatcherOnTheAmountChange(Action<int> isAmountChanged)
        {
            _onAmountChanged += isAmountChanged;
        }
        
        public void UnregisterWatcherOnTheAmountChange(Action<int> isAmountChanged)
        {
            _onAmountChanged -= isAmountChanged;
        }

        public bool TryAmountChangeOnThe(int value)
        {
            if (SoulsOfTheDungeon + value < 0) return false;
            SoulsOfTheDungeon += value;
            _onAmountChanged?.Invoke(SoulsOfTheDungeon);
            return true;
        }
    }
}
