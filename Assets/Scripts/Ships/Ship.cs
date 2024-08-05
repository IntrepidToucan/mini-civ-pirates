using UnityEngine;

namespace Ships
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ship : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<SpriteRenderer>().sortingLayerID = SortingLayer.NameToID("Actors");
            GetComponent<Rigidbody2D>().gravityScale = 0f;
        }
    }
}
