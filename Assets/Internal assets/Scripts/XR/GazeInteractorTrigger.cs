using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace XR
{
    public class GazeInteractorTrigger : MonoBehaviour
    {
        // [SerializeField] private XRSimpleInteractable xrSimpleInteractable;
        // [SerializeField] private GameObject menu;
        //
        // private void Awake()
        // {
        //     xrSimpleInteractable.hoverEntered.AddListener(arg0 => menu.SetActive(true));
        //     xrSimpleInteractable.hoverExited.AddListener(arg0 => menu.SetActive(false));
        // }

        public void AddHoverEntered(UnityAction<HoverEnterEventArgs> unityAction)
        {
            if (TryGetComponent<XRSimpleInteractable>(out var xrSimpleInteractable))
                xrSimpleInteractable.hoverEntered.AddListener(unityAction);
        }

        public void AddHoverExited(UnityAction<HoverExitEventArgs> unityAction)
        {
            if (TryGetComponent<XRSimpleInteractable>(out var xrSimpleInteractable))
                xrSimpleInteractable.hoverExited.AddListener(unityAction);
        }

        public void RemoveHoverEntered(UnityAction<HoverEnterEventArgs> unityAction)
        {
            if (TryGetComponent<XRSimpleInteractable>(out var xrSimpleInteractable))
                xrSimpleInteractable.hoverEntered.RemoveListener(unityAction);
        }

        public void RemoveHoverExited(UnityAction<HoverExitEventArgs> unityAction)
        {
            if (TryGetComponent<XRSimpleInteractable>(out var xrSimpleInteractable))
                xrSimpleInteractable.hoverExited.RemoveListener(unityAction);
        }
    }
}