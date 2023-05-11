using System.Collections.Generic;
using Data.Dynamic;
using Services.PersistentProgress;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Item.Weapon
{
    public class WeaponManagerMainLocation : MonoBehaviour, IProgressSavable, IWeaponManager
    {
        public WeaponType SelectedWeaponType { get; set; }
        public Transform SocketTransform { get; set; }

        public Dictionary<WeaponType, Transform> WeaponsTransform { get; } = new()
        {
            { WeaponType.Sword, null },
            { WeaponType.Ax, null },
            { WeaponType.Hammer, null }
        };

        public void SetUp(GameObject socketTransform, params GameObject[] weapons)
        {
            SocketTransform = socketTransform.transform;
            foreach (var weapon in weapons)
            {
                if (!weapon.TryGetComponent<Weapon>(out var weaponComponent)) continue;

                WeaponsTransform[weaponComponent.WeaponType] = weapon.transform;
                if (weapon.TryGetComponent<XRGrabInteractable>(out var grabComponent))
                {
                    grabComponent.selectEntered.AddListener(
                        (SelectEnterEventArgs arg0) => ChangeSelectedWeapon(weaponComponent.WeaponType));
                }
            }
        }

        public void UpdateProgress(Progress progress)
        {
            progress.selectedWeapon.weaponType = SelectedWeaponType;
        }

        public void MoveWeaponInSocket(WeaponType selectedWeaponType)
        {
            WeaponsTransform[selectedWeaponType].GetComponent<XRGrabInteractable>().enabled = false;
            WeaponsTransform[selectedWeaponType].position = SocketTransform.position;
            WeaponsTransform[selectedWeaponType].GetComponent<XRGrabInteractable>().enabled = true;
        }

        public void LoadProgress(Progress progress)
        {
            SelectedWeaponType = progress.selectedWeapon.weaponType;
            MoveWeaponInSocket(SelectedWeaponType);
        }

        private void ChangeSelectedWeapon(WeaponType newSelectedWeaponType)
        {
            SelectedWeaponType = newSelectedWeaponType;
        }
    }
}