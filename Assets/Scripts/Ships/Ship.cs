using System;
using System.Collections.Generic;
using Data;
using Managers;
using UnityEngine;

namespace Ships
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ship : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private ShipData shipData;
        
        private List<GameObject> _ghosts;
        private Rigidbody2D _rigidBody;
        private Vector3Int _targetCell;

        public string GetInGameName() => shipData.inGameName;
        public int GetFood() => shipData.food;
        
        public void SetTargetCell(Vector3Int pos) => _targetCell = pos;
        
        private void Awake()
        {
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Actors");

            shipData = Instantiate(shipData);
            
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.gravityScale = 0f;

            _ghosts = transform.parent?.gameObject.GetComponent<Ship>() ? null : new List<GameObject>();
        }

        private void Start()
        {
            _targetCell = TileManager.Instance.WaterTilemap.WorldToCell(transform.position);
            
            if (IsGhost())
            {
                _rigidBody.bodyType = RigidbodyType2D.Static;
                return;
            }
            
            _rigidBody.bodyType = RigidbodyType2D.Kinematic;
            
            CreateGhostActors();
            UpdatePosition();
        }

        private float _foodCountdown;

        private void Update()
        {
            if (IsGhost()) return;
            if (shipData.food <= 0) return;
            
            _foodCountdown += Time.deltaTime;

            if (_foodCountdown >= 2f)
            {
                shipData.food = Math.Max(shipData.food - 1, 0);
                EventManager.TriggerShipFoodChange(this, shipData.food);
                _foodCountdown = 0f;
            }
            
            UpdatePosition();
        }

        private void OnDrawGizmos()
        {
            if (IsGhost()) return;
            if (TileManager.Instance == null) return;
            
            var worldPos = TileManager.Instance.WaterTilemap.CellToWorld(_targetCell);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, worldPos);
            Gizmos.DrawWireSphere(worldPos, 0.5f);
        }

        private bool IsGhost() => _ghosts == null;

        private void CreateGhostActors()
        {
            foreach (var pos in TileManager.Instance.GetGhostGridPositions(transform.position))
            {
                _ghosts.Add(Instantiate(_ghosts.Count > 0 ? _ghosts[0] : gameObject,
                    pos, transform.rotation, transform));
            }
        }

        private void UpdatePosition()
        {
            TileManager.Instance.ExploreWorldPosition(transform.position);
            
            var ghostPositions = TileManager.Instance.GetGhostGridPositions(transform.position);

            for (var i = 0; i < _ghosts.Count; i++)
            {
                _ghosts[i].transform.position = ghostPositions[i];
                _ghosts[i].transform.rotation = transform.rotation;
            }
            
            var diff = (Vector3)(Vector2)(TileManager.Instance.WaterTilemap.GetCellCenterWorld(_targetCell) - transform.position);

            if (diff.sqrMagnitude < 0.01) return;

            transform.position += diff.normalized * 5f * Time.deltaTime;
            
            var angle = (Mathf.Atan2(diff.y, diff.x) - Mathf.Atan2(-1, 0)) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0,  angle < 0 ? 360 + angle : angle);
        }
    }
}
