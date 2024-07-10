using UnityEngine;

namespace Utilities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HighlightCollider : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        
        public void Highlight()
        {
            _spriteRenderer.enabled = true;
        }
        
        public void Unhighlight()
        {
            _spriteRenderer.enabled = false;
        }
        
        private void Awake()
        {
            var layer= LayerMask.NameToLayer("Highlightable");

            if (gameObject.layer != layer)
            {
                Debug.LogError("Incorrect layer");
                gameObject.layer = layer;
            }

            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
