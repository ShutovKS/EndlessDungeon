using System;

namespace Data.Dynamic.PlayerData
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerProgress()
        {
            selectedWeapon = new SelectedWeapon();
        }

        public SelectedWeapon selectedWeapon;
    }
}