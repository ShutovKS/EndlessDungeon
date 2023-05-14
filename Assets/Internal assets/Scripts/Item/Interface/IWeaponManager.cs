using System.Collections.Generic;
using Data.Static;
using Item.Weapon;
using Services.PersistentProgress;
using UnityEngine;

namespace Item
{
    public interface IWeaponManager : IProgressLoadable
    {
        WeaponType SelectedWeaponType { get; set; }
        Transform SocketTransform { get; set; }
        Dictionary<WeaponType, Transform> WeaponsTransform { get; }

        void SetUp(GameObject socketTransform, PlayerStaticDefaultData playerStaticDefaultData,
            params GameObject[] weapons);
        void MoveWeaponInSocket(WeaponType selectedWeaponType);
    }
}