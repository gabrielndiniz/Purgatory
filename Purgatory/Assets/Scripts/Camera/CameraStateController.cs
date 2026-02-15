using Cinemachine;
using UnityEngine;

namespace Survivor.Camera
{
    public class CameraStateController : MonoBehaviour
    {
        [Header("Cameras")]
        [SerializeField] private CinemachineVirtualCamera cameraAction;

        [Header("Priorities")]
        [SerializeField] private int activePriority = 20;
        [SerializeField] private int inactivePriority = 0;


        public void ActivateActionCamera()
        {
            if (cameraAction == null)
            {
                Debug.LogWarning("CameraStateController: CameraAction is null.");
                return;
            }

            cameraAction.Priority = activePriority;
        }

        public void DeactivateActionCamera()
        {
            if (cameraAction == null)
                return;

            cameraAction.Priority = inactivePriority;
        }
    }
}
