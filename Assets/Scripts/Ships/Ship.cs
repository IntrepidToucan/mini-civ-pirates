using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Ships
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Ship : MonoBehaviour
    {
        private List<GameObject> _ghosts;
        private Rigidbody2D _rigidBody;
        private Vector3 _targetPos;

        public void SetTargetPosition(Vector3 pos)
        {
            _targetPos = pos;
        }
        
        private void Awake()
        {
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Actors");
            
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.gravityScale = 0f;

            _ghosts = new List<GameObject>();
            _targetPos = transform.position;
        }

        private void Start()
        {
            if (IsGhost())
            {
                _rigidBody.bodyType = RigidbodyType2D.Static;
                GetComponent<CapsuleCollider2D>().enabled = false;
                return;
            }
            
            CreateGhostActors();
            
            TileManager.Instance.ExploreWorldPosition(transform.position);
        }

        private void Update()
        {
            if (IsGhost()) return;
            
            var diff = _targetPos - transform.position;

            if (diff.sqrMagnitude < 1) return;

            _rigidBody.velocity = diff.normalized * 5f;
            _rigidBody.SetRotation(Quaternion.LookRotation(diff));

            var ghostPositions = TileManager.Instance.GetGhostGridPositions(transform.position);

            for (var i = 0; i < _ghosts.Count; i++)
            {
                _ghosts[i].transform.position = ghostPositions[i];
                _ghosts[i].transform.rotation = transform.rotation;
            }
            
            TileManager.Instance.ExploreWorldPosition(transform.position);
        }

        private void CreateGhostActors()
        {
            foreach (var pos in TileManager.Instance.GetGhostGridPositions(transform.position))
            {
                _ghosts.Add(Instantiate(gameObject, pos, transform.rotation));
            }
        }

        private bool IsGhost()
        {
            var mainTilemap = TileManager.Instance.WaterTilemap;

            return !mainTilemap.HasTile(mainTilemap.WorldToCell(transform.position));
        }
    }
}
