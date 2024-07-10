using UnityEngine;

namespace Settlements
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Settlement : MonoBehaviour
    {
        public struct HarborData
        {
            public HarborData(Vector2 position)
            {
                Position = position;
            }
            
            public Vector2 Position;
        }
        
        [Header("Info")]
        [SerializeField] private string settlementName;

        public HarborData[] Harbors { get; private set; }

        private void Awake()
        {
            var sortingLayer = SortingLayer.NameToID("Actors");
            var spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer.sortingLayerID != sortingLayer)
            {
                Debug.LogError("Incorrect sorting layer");
                spriteRenderer.sortingLayerID = sortingLayer;
            }

            Harbors = new[] {
                new HarborData(new Vector2(-0.62f,1.33f)),
                new HarborData(),
                new HarborData(),
                new HarborData(),
                new HarborData(),
                new HarborData()
            };
        }
    }
}
