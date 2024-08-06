using System;
using Cameras;
using Managers;
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

        private GameObject _activeShip;
        private Vector3 _targetPos;

        private void Start()
        {
            _activeShip = GameObject.Find("Ship_Appletown");
        }

        private void Update()
        {
            var diff = _targetPos - _activeShip.transform.position;

            if (diff.sqrMagnitude < 1) return;

            var rigidBody = _activeShip.GetComponent<Rigidbody2D>();
            rigidBody.velocity = diff.normalized * 10f;
            rigidBody.SetRotation(Quaternion.LookRotation(diff));
        }

        public void HandleSelect(Vector3 mousePos)
        {
            var worldPosition = MainCamera.Instance.Camera.ScreenToWorldPoint(mousePos);
            // var cellPosition = TileManager.Instance.WaterTilemap.layoutGrid.LocalToCell(worldPosition);
            
            // Debug.Log(cellPosition);
            _targetPos = worldPosition;
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
