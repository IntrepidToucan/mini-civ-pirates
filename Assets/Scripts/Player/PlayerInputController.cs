using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputController : MonoBehaviour
    {
        private InputAction _panCameraAction;
        private InputAction _selectAction;
        private InputAction _zoomCameraAction;
        
        private void Start()
        {
            _panCameraAction = Player.Instance.PlayerInput.actions.FindAction("PanCamera");
            _zoomCameraAction = Player.Instance.PlayerInput.actions.FindAction("ZoomCamera");
            
            _selectAction = Player.Instance.PlayerInput.actions.FindAction("Select");
            _selectAction.performed += context =>
            {
                Player.Instance.MovementController.HandleSelect(Mouse.current.position.ReadValue());
            };
        }

        private void Update()
        {
            var panCameraValue = _panCameraAction.ReadValue<Vector2>();
            var zoomCameraValue = _zoomCameraAction.ReadValue<float>();
            
            // TileManager.Instance.HandleMousePosition(Mouse.current.position.ReadValue());

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
