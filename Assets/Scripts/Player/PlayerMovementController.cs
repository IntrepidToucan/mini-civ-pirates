using Cameras;
using Managers;
using Ships;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Camera Movement")]
        [SerializeField, Range(0.1f, 10f)] private float cameraOrthoSizeMin = 3f;
        [SerializeField, Range(0.1f, 12f)] private float cameraOrthoSizeMax = 10f;
        [SerializeField, Range(0.1f, 10f)] private float cameraPanSpeed = 6f;
        [SerializeField, Range(0.1f, 10f)] private float cameraZoomSpeed = 2f;
        
        [Header("Debug")]
        [SerializeField] private bool debugSelection;

        private GameObject _activeObject;
        
        public void HandleSelect(Vector3 mousePos)
        {
            if (debugSelection)
            {
                Debug.Log($"Select mouse pos: {mousePos}");
            }

            var ray = MainCamera.Instance.Camera.ScreenPointToRay(mousePos);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, float.PositiveInfinity,
                LayerMask.GetMask("ClickableActors"));
            
            if (hit)
            {
                if (debugSelection)
                {
                    Debug.Log($"Hit actor: {hit.transform.gameObject.name}");
                }
                
                _activeObject = hit.transform.gameObject;
                return;
            }

            if (_activeObject == null) return;
            
            if (debugSelection)
            {
                Debug.Log($"Active object: {_activeObject.name}");
            }
            
            if (_activeObject.TryGetComponent<Ship>(out var ship))
            {
                var worldPos = MainCamera.Instance.Camera.ScreenToWorldPoint(mousePos);
                ship.SetTargetPosition(worldPos);
            }
            else
            {
                Debug.LogError($"Unhandled active object: {_activeObject.name}");
            }
        }

        public void PanCamera(Vector2 panCameraValue)
        {
            var panVector = (panCameraValue.sqrMagnitude > 1f ? panCameraValue.normalized : panCameraValue)
                            * (cameraPanSpeed * Time.deltaTime);
            var newPosition = (Vector2)Player.Instance.transform.position + panVector;
            var tilemapBounds = TileManager.Instance.WaterTilemap.cellBounds;
            var warpCamera = false;

            if (newPosition.x < tilemapBounds.xMin)
            {
                newPosition.x = tilemapBounds.xMax;
                warpCamera = true;
            } else if (newPosition.x > tilemapBounds.xMax)
            {
                newPosition.x = tilemapBounds.xMin;
                warpCamera = true;
            }
            
            if (newPosition.y < tilemapBounds.yMin)
            {
                newPosition.y = tilemapBounds.yMax;
                warpCamera = true;
            } else if (newPosition.y > tilemapBounds.yMax)
            {
                newPosition.y = tilemapBounds.yMin;
                warpCamera = true;
            }

            if (warpCamera)
            {
                PlayerFollowCamera.Instance.CineVirtualCamera.OnTargetObjectWarped(Player.Instance.transform,
                    (Vector3)newPosition - Player.Instance.transform.position);
            }

            Player.Instance.transform.position = newPosition;
        }
        
        public void ZoomCamera(float zoomCameraValue)
        {
            var newOrthoSize = PlayerFollowCamera.Instance.CineVirtualCamera.m_Lens.OrthographicSize +
                               zoomCameraValue * cameraZoomSpeed * Time.deltaTime;

            PlayerFollowCamera.Instance.CineVirtualCamera.m_Lens.OrthographicSize =
                Mathf.Clamp(newOrthoSize, cameraOrthoSizeMin, cameraOrthoSizeMax);
        }
    }
}
