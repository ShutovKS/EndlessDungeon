using System.Collections.Generic;
using Services.PersistentProgress;
using UnityEngine;

namespace Item.Weapon
{
    public interface IWeaponManager : IProgressLoadable
    {
        WeaponType SelectedWeaponType { get; set; }
        Transform SocketTransform { get; set; }
        Dictionary<WeaponType, Transform> WeaponsTransform { get; }
        void SetUp(GameObject socketTransform, params GameObject[] weapons);
        void MoveWeaponInSocket(WeaponType selectedWeaponType);
    }
}