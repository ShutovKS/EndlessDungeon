#region

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#endregion

namespace Item.Weapon
{
    public class Weapon : MonoBehaviour, IWeapon, IItemDamage
    {
        private void Start()
        {
            if (TryGetComponent<XRGrabInteractable>(out var xrGrabInteractable))
            {
                xrGrabInteractable.selectEntered.AddListener(_ => ItemIsDamage(true));
                xrGrabInteractable.selectExited.AddListener(_ => ItemIsDamage(false));
            }
        }

        public float Damage { get; private set; }
        public bool IsDamage { get; private set; }

        public void SetDamage(float value)
        {
            Damage = value;
        }

        public void ItemIsDamage(bool value)
        {
            IsDamage = value;
        }

        [field: SerializeField] public WeaponType WeaponType { get; private set; }
    }
}
