using Cameras;
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
        
        public void PanCamera(Vector2 panCameraValue)
        {
            var panVector = (panCameraValue.sqrMagnitude > 1f ? panCameraValue.normalized : panCameraValue)
                            * (cameraPanSpeed * Time.deltaTime);
            var newPosition = (Vector2)Player.Instance.transform.position + panVector;
            var tilemapBounds = Player.Instance.WaterTilemap.cellBounds;
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
                VirtualCamera.Instance.CineVirtualCamera.OnTargetObjectWarped(Player.Instance.transform,
                    (Vector3)newPosition -  Player.Instance.transform.position);
            }

            Player.Instance.transform.position = newPosition;
        }
        
        public void ZoomCamera(float zoomCameraValue)
        {
            var newOrthoSize = VirtualCamera.Instance.CineVirtualCamera.m_Lens.OrthographicSize +
                               zoomCameraValue * cameraZoomSpeed * Time.deltaTime;

            VirtualCamera.Instance.CineVirtualCamera.m_Lens.OrthographicSize =
                Mathf.Clamp(newOrthoSize, cameraOrthoSizeMin, cameraOrthoSizeMax);
        }
    }
}
