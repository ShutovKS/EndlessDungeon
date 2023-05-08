using System;
using System.Collections.Generic;
using Data.Dynamic.PlayerData;
using Services.PersistentProgress;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Item.Weapon
{
    public class WeaponManager : MonoBehaviour, IProgressSavable, IProgressLoadable
    {
        private WeaponType _selectedWeaponType;
        private Transform _socketTransform;

        private readonly Dictionary<WeaponType, Transform> _weaponsTransform = new()
        {
            { WeaponType.Sword, null },
            { WeaponType.Ax, null },
            { WeaponType.Hammer, null }
        };

        public void SetUp(GameObject socketTransform, params GameObject[] weapons)
        {
            _socketTransform = socketTransform.transform;
            foreach (var weapon in weapons)
            {
                if (weapon.TryGetComponent<Weapon>(out var weaponComponent))
                {
                    _weaponsTransform[weaponComponent.WeaponType] = weapon.transform;
                    if (weapon.TryGetComponent<XRGrabInteractable>(out var grabComponent))
                    {
                        grabComponent.selectEntered.AddListener((SelectEnterEventArgs arg0) =>
                            ChangeSelectedWeapon(weaponComponent.WeaponType));
                    }
                }
            }
        }

        private void Start()
        {
            MoveWeaponInSocket(_selectedWeaponType);
        }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.selectedWeapon.weaponType = _selectedWeaponType;
            MoveWeaponInSocket(_selectedWeaponType);
        }

        public void LoadProgress(PlayerProgress playerProgress)
        {
            var weaponType = playerProgress.selectedWeapon.weaponType;
            _selectedWeaponType = weaponType;
            MoveWeaponInSocket(_selectedWeaponType);
        }

        private void ChangeSelectedWeapon(WeaponType newSelectedWeaponType)
        {
            _selectedWeaponType = newSelectedWeaponType;
        }

        private void MoveWeaponInSocket(WeaponType selectedWeaponType)
        {
            _weaponsTransform[selectedWeaponType].position = _socketTransform.position;
        }
    }
}