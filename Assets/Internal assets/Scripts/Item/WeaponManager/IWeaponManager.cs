#region

using System.Collections.Generic;
using Item.Weapon;
using Services.Watchers.PersistentProgressWatcher;
using UnityEngine;

#endregion

namespace Item.WeaponManager
{
    public interface IWeaponManager : IProgressLoadableWatcher
    {
        WeaponType SelectedWeaponType { get; }
        float DamageDefault { get; }
        Transform SocketTransform { get; }
        Dictionary<WeaponType, Transform> WeaponsTransform { get; }

        void SetUp(Transform socketTransform, float damage, params GameObject[] weapons);

        void MoveWeaponInSocket(WeaponType selectedWeaponType);
    }
}
