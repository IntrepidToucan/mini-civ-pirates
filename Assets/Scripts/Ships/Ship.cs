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

        public void SetTargetPosition(Vector3 pos) => _targetPos = pos;
        
        private void Awake()
        {
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Actors");
            
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.gravityScale = 0f;

            _ghosts = transform.parent?.gameObject.GetComponent<Ship>() ? null : new List<GameObject>();
            _targetPos = transform.position;
        }

        private void Start()
        {
            if (IsGhost())
            {
                _rigidBody.bodyType = RigidbodyType2D.Static;
                return;
            }
            
            CreateGhostActors();
            UpdatePosition();
        }

        private void Update()
        {
            if (IsGhost()) return;
            
            UpdatePosition();
        }

        private void OnDrawGizmos()
        {
            if (IsGhost()) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _targetPos);
            Gizmos.DrawWireSphere(_targetPos, 0.5f);
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
            
            var diff = (Vector3)(Vector2)(_targetPos - transform.position);

            if (diff.sqrMagnitude < 0.01) return;

            transform.position += diff.normalized * 5f * Time.deltaTime;
            
            var angle = (Mathf.Atan2(diff.y, diff.x) - Mathf.Atan2(-1, 0)) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0,  angle < 0 ? 360 + angle : angle);
        }
    }
}
