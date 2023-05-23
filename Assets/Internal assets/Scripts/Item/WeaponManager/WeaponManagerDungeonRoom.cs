#region

using System.Collections.Generic;
using Data.Dynamic;
using Item.Weapon;
using Skill;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#endregion

namespace Item.WeaponManager
{
    public class WeaponManagerDungeonRoom : MonoBehaviour, IWeaponManager
    {
        public WeaponType SelectedWeaponType { get; private set; }
        public Transform SocketTransform { get; private set; }
        public float DamageDefault { get; private set; }

        public Dictionary<WeaponType, Transform> WeaponsTransform { get; } = new Dictionary<WeaponType, Transform>
        {
            [WeaponType.Ax] = null,
            [WeaponType.Hammer] = null,
            [WeaponType.Sword] = null
        };

        public void SetUp(Transform socketTransform, float damage, params GameObject[] weapons)
        {
            SocketTransform = socketTransform;
            DamageDefault = damage;
            foreach (var weapon in weapons)
            {
                if (weapon.TryGetComponent<Weapon.Weapon>(out var weaponComponent))
                {
                    WeaponsTransform[weaponComponent.WeaponType] = weapon.transform;
                }
            }
        }

        public void MoveWeaponInSocket(WeaponType selectedWeaponType)
        {
            if (WeaponsTransform[selectedWeaponType] == null) return;
            WeaponsTransform[selectedWeaponType].GetComponent<Rigidbody>().isKinematic = true;
            WeaponsTransform[selectedWeaponType].GetComponent<XRGrabInteractable>().enabled = false;
            WeaponsTransform[selectedWeaponType].position = SocketTransform.position;
            WeaponsTransform[selectedWeaponType].GetComponent<XRGrabInteractable>().enabled = true;
            WeaponsTransform[selectedWeaponType].GetComponent<Rigidbody>().isKinematic = false;
        }

        public void LoadProgress(Progress progress)
        {
            var skills = progress.skillsLevel.skills;
            SelectedWeaponType = progress.selectedWeapon.weaponType;

            foreach (var (weaponType, weaponTransform) in WeaponsTransform)
            {
                if (weaponTransform == null) return;
                if (weaponType == SelectedWeaponType)
                {
                    weaponTransform.GetComponent<IItemDamage>().SetDamage(
                        (DamageDefault + skills[SkillType.StrengthCount]) *
                        (1 + skills[SkillType.StrengthPercent]));
                }
                else
                {
                    weaponTransform.gameObject.SetActive(false);
                }

                MoveWeaponInSocket(SelectedWeaponType);
            }
        }
    }
}
