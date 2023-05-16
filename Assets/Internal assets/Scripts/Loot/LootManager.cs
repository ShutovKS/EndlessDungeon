using System;
using Data.Dynamic;
using Services.PersistentProgress;
using UnityEngine;

namespace Loot
{
    public class LootManager : MonoBehaviour, IProgressLoadable, IProgressSavable
    {
        public int SoulsOfTheDungeon { get; private set; }
        private Action<int> _isAmountChanged;

        public void RegisterOnTheAmountChange(Action<int> isAmountChanged)
        {
            _isAmountChanged += isAmountChanged;
        }

        public bool TryAmountChangeOnThe(int value)
        {
            if (SoulsOfTheDungeon + value < 0) return false;
            SoulsOfTheDungeon += value;
            _isAmountChanged?.Invoke(SoulsOfTheDungeon);
            return true;
        }

        public void LoadProgress(Progress progress)
        {
            SoulsOfTheDungeon = progress.lootData.soulsOfTheDungeon;
            _isAmountChanged?.Invoke(SoulsOfTheDungeon);
        }

        public void UpdateProgress(Progress progress)
        {
            progress.lootData.soulsOfTheDungeon = SoulsOfTheDungeon;
        }
    }
}