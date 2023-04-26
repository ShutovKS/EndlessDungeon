using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Item
{
    public class ItemDamage : MonoBehaviour
    {
        public bool isDamage = false;
        public int Damage = 20;

        private XRGrabInteractable _xrGrabInteractable;

        private void Start()
        {
            _xrGrabInteractable = GetComponent<XRGrabInteractable>();

        }

        public void ItemIsDamage()
        {
            isDamage = true;
        }

        public void Print(string str)
        {
            Debug.Log(str);
        }

        //https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/manual/interactable-events.html

        /*
         * 1 - при первом наведении
         * 2 - 
         * 3 - при наведении
         * 4 - при прекращении наведение
         * 5 - при самом начале начале взятия 
         * 6 - при самом прекращении взятия
         * 7 - при начале взятия
         * 8 - при прекращении взятия
         * 9 - при взаимодействии при взятии
         * 10 - при прекращении взаимодействия при взятии
         */

    }
}