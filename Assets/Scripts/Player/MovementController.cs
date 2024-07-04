using Cameras;
using UnityEngine;

namespace Player
{
    public class MovementController : MonoBehaviour
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

            Player.Instance.transform.position = new Vector3(
                Mathf.Clamp(newPosition.x, tilemapBounds.xMin, tilemapBounds.xMax),
                Mathf.Clamp(newPosition.y, tilemapBounds.yMin, tilemapBounds.yMax), 0f);
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
