using Cameras;
using Settlements;
using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerInteractionController : MonoBehaviour
    {
        private HighlightCollider _prevHighlightCollider;

        public void UpdateHighlight(Vector2 mousePosition)
        {
            var hit = GetMouseHit(mousePosition);
            var currentHighlightCollider = hit ? hit.transform.GetComponent<HighlightCollider>() : null;

            if (currentHighlightCollider != _prevHighlightCollider)
            {
                _prevHighlightCollider?.Unhighlight();
                currentHighlightCollider?.Highlight();
            }

            _prevHighlightCollider = currentHighlightCollider;
        }

        public static void TrySelect(Vector2 mousePosition)
        {
            var hit = GetMouseHit(mousePosition);

            if (!hit) return;

            var settlement = hit.transform.GetComponentInParent<Settlement>();

            if (settlement is not null)
            {
                Player.Instance.UiController.ShowSettlementMenu(settlement);
            }
        }

        private static RaycastHit2D GetMouseHit(Vector2 mousePosition)
        {
            var ray = MainCamera.Instance.Camera.ScreenPointToRay(mousePosition);
            
            return Physics2D.Raycast(ray.origin, ray.direction,
                float.PositiveInfinity, LayerMask.GetMask("Highlightable"));
        }
    }
}
