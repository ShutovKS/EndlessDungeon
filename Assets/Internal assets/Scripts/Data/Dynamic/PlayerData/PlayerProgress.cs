using System;

namespace Data.Dynamic.PlayerData
{
    [Serializable]
    public class PlayerProgress
    {
        public SelectedWeapon selectedWeapon;

        public PlayerProgress()
        {
            selectedWeapon = new SelectedWeapon();
        }
    }
}