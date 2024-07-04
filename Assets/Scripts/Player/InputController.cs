using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputController : MonoBehaviour
    {
        private InputAction _panCameraAction;
        private InputAction _zoomCameraAction;
        
        private void Start()
        {
            _panCameraAction = Player.Instance.PlayerInput.actions.FindAction("PanCamera");
            _zoomCameraAction = Player.Instance.PlayerInput.actions.FindAction("ZoomCamera");
        }

        private void Update()
        {
            var panCameraValue = _panCameraAction.ReadValue<Vector2>();
            var zoomCameraValue = _zoomCameraAction.ReadValue<float>();

            if (panCameraValue != Vector2.zero)
            {
                Player.Instance.MovementController.PanCamera(panCameraValue);
            }
            
            if (!Mathf.Approximately(zoomCameraValue, 0f))
            {
                Player.Instance.MovementController.ZoomCamera(zoomCameraValue);
            }
        }
    }
}
