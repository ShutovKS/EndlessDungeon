using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Item.Weapon
{
    public class Weapon : MonoBehaviour, IItemDamage, IWeapon
    {
        public WeaponType WeaponType => weaponType;
        [SerializeField] private WeaponType weaponType;
        
        [field: SerializeField]
        public bool IsDamage { get; set; }
        
        [field: SerializeField]
        public float Damage { get; set; } = 10;

        private void Start()
        {
            var grabInteractable = GetComponent<XRGrabInteractable>();

            grabInteractable.selectEntered.AddListener((SelectEnterEventArgs arg0) => ItemIsDamage(true));
            grabInteractable.selectExited.AddListener((SelectExitEventArgs arg0) => ItemIsDamage(false));
            
        }
        public void ItemIsDamage(bool value) => IsDamage = value;
    }
}