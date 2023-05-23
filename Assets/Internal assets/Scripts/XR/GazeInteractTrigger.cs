#region

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

#endregion

namespace XR
{
    public class GazeInteractTrigger : MonoBehaviour
    {
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
