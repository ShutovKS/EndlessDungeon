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
        public float Damage { get; set; }

        private void Start()
        {
            var grabInteractable = GetComponent<XRGrabInteractable>();

            grabInteractable.selectEntered.AddListener(_ => ItemIsDamage(true));
            grabInteractable.selectExited.AddListener(_ => ItemIsDamage(false));
            
        }
        public void ItemIsDamage(bool value) => IsDamage = value;
    }
}