using UnityEngine;

namespace Ships
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Ship : MonoBehaviour
    {
        private void Awake()
        {
            var sortingLayer = SortingLayer.NameToID("Actors");
            var spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer.sortingLayerID != sortingLayer)
            {
                Debug.LogError("Incorrect sorting layer");
                spriteRenderer.sortingLayerID = sortingLayer;
            }
        }
    }
}
