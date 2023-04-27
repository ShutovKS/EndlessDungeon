using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Item.Weapon
{
    public class Weapon : MonoBehaviour, IItemDamage, IWeapon
    {
        public WeaponType WeaponType => weaponType;
        [SerializeField] private WeaponType weaponType;
        public bool IsDamage { get; set; }
        public float Damage { get; set; } = 10;

        private void Start()
        {
            var grabInteractable = GetComponent<XRGrabInteractable>();

            grabInteractable.selectEntered.AddListener(ActivatorWeapon);
            grabInteractable.selectEntered.AddListener(SetWeaponType);
            grabInteractable.selectExited.AddListener(DeactivateWeapon);
        }

        private void SetWeaponType(SelectEnterEventArgs arg0) => SetWeaponTypeInDefault();
        private void ActivatorWeapon(SelectEnterEventArgs arg0) => ItemIsDamage(true);
        private void DeactivateWeapon(SelectExitEventArgs arg0) => ItemIsDamage(false);

        public void SetWeaponTypeInDefault() => WeaponDefault.WeaponType = weaponType;
        public void ItemIsDamage(bool value) => IsDamage = value;
    }
}