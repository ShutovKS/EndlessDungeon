using System.Collections.Generic;
using Data.Dynamic;
using Data.Dynamic.Player;
using Data.Static;
using Services.PersistentProgress;
using Skill;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Item.Weapon
{
    public class WeaponManagerMainLocation : MonoBehaviour, IProgressSavable, IWeaponManager
    {
        public WeaponType SelectedWeaponType { get; set; }
        public Transform SocketTransform { get; set; }
        private PlayerStaticDefaultData _defaultData;

        public Dictionary<WeaponType, Transform> WeaponsTransform { get; } = new()
        {
            [WeaponType.Sword] = null,
            [WeaponType.Ax] = null,
            [WeaponType.Hammer] = null
        };

        public void SetUp(GameObject socketTransform, PlayerStaticDefaultData playerStaticDefaultData,
            params GameObject[] weapons)
        {
            SocketTransform = socketTransform.transform;
            _defaultData = playerStaticDefaultData;
            foreach (var weapon in weapons)
            {
                if (!weapon.TryGetComponent<Weapon>(out var weaponComponent)) continue;
                WeaponsTransform[weaponComponent.WeaponType] = weapon.transform;
                if (weapon.TryGetComponent<XRGrabInteractable>(out var grabComponent))
                    grabComponent.selectEntered.AddListener(_ => ChangeSelectedWeapon(weaponComponent.WeaponType));
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
            var skills = progress.skillsLevel.Skills;
            var damage = _defaultData.DamagePoints;

            foreach (var (_, weaponTransform) in WeaponsTransform)
            {
                weaponTransform.GetComponent<IItemDamage>().Damage =
                    (damage + skills[SkillType.STREANGHT_Count]) *
                    (1 + skills[SkillType.STREANGHT_Percent]);
            }

            SelectedWeaponType = progress.selectedWeapon.weaponType;
            MoveWeaponInSocket(SelectedWeaponType);
        }

        private void ChangeSelectedWeapon(WeaponType newSelectedWeaponType)
        {
            SelectedWeaponType = newSelectedWeaponType;
        }
    }
}