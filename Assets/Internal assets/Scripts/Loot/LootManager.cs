using System;
using Data.Dynamic;
using Services.PersistentProgress;
using UnityEngine;

namespace Loot
{
    public class LootManager : MonoBehaviour ,IProgressLoadable, IProgressSavable
    {
        private int _soulsOfTheDungeon;
        private Action<int> _isAmountChanged;

        public void RegisterOnTheAmountChange(Action<int> isAmountChanged)
        {
            _isAmountChanged = isAmountChanged;
        }

        public bool TryAmountChangeOnThe(int value)
        {
            if (_soulsOfTheDungeon + value < 0) return false;
            _soulsOfTheDungeon += value;
            _isAmountChanged?.Invoke(_soulsOfTheDungeon);
            return true;
        }

        public void LoadProgress(Progress progress)
        {
            _soulsOfTheDungeon = progress.lootData.soulsOfTheDungeon;
            _isAmountChanged?.Invoke(_soulsOfTheDungeon);
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lootData.soulsOfTheDungeon = _soulsOfTheDungeon;
        }
    }
}