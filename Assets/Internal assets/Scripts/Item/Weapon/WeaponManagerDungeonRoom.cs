using System.Collections.Generic;
using Data.Dynamic;
using Data.Dynamic.Player;
using Data.Static;
using Skill;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Item.Weapon
{
    public class WeaponManagerDungeonRoom : MonoBehaviour, IWeaponManager
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

        public void SetUp(Transform socketTransform, PlayerStaticDefaultData playerStaticDefaultData,
            params GameObject[] weapons)
        {
            SocketTransform = socketTransform;
            _defaultData = playerStaticDefaultData;
            foreach (var weapon in weapons)
            {
                if (!weapon.TryGetComponent<Weapon>(out var weaponComponent)) continue;
                WeaponsTransform[weaponComponent.WeaponType] = weapon.transform;
                if (weapon.TryGetComponent<XRGrabInteractable>(out var grabComponent))
                    grabComponent.selectEntered.AddListener(_ => ChangeSelectedWeapon(weaponComponent.WeaponType));
            }
        }

        public void MoveWeaponInSocket(WeaponType selectedWeaponType)
        {
            WeaponsTransform[selectedWeaponType].GetComponent<Rigidbody>().isKinematic = true;
            WeaponsTransform[selectedWeaponType].GetComponent<XRGrabInteractable>().enabled = false;
            WeaponsTransform[selectedWeaponType].position = SocketTransform.position;
            WeaponsTransform[selectedWeaponType].GetComponent<XRGrabInteractable>().enabled = true;
            WeaponsTransform[selectedWeaponType].GetComponent<Rigidbody>().isKinematic = false;
        }

        public void LoadProgress(Progress progress)
        {
            var skills = progress.skillsLevel.Skills;
            var damage = _defaultData.DamagePoints;
            SelectedWeaponType = progress.selectedWeapon.weaponType;

            foreach (var (weaponType, weaponTransform) in WeaponsTransform)
            {
                weaponTransform.GetComponent<IItemDamage>().Damage =
                    (damage + skills[SkillType.STREANGHT_Count]) *
                    (1 + skills[SkillType.STREANGHT_Percent]);

                weaponTransform.gameObject.SetActive(weaponType == SelectedWeaponType);
            }

            MoveWeaponInSocket(SelectedWeaponType);
        }

        private void ChangeSelectedWeapon(WeaponType newSelectedWeaponType)
        {
            SelectedWeaponType = newSelectedWeaponType;
        }
    }
}