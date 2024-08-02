using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;

namespace Managers
{
    public class TileManager : PersistedSingleton<TileManager>
    {
        public Tilemap WaterTilemap { get; private set; }

        public void HandleMousePosition(Vector2 mousePosition)
        {
        }

        public void HandleSelect(Vector2 mousePosition)
        {
            Debug.Log(mousePosition);
        }
        
        private void Start()
        {
            WaterTilemap = GameObject.Find("/Grid/Water").GetComponent<Tilemap>();
            
            var tilemapBounds = WaterTilemap.cellBounds;
            var grid = GameObject.Find("/Grid");
            
            Instantiate(grid, grid.transform.position + grid.transform.right * tilemapBounds.size.x, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + -grid.transform.right * tilemapBounds.size.x, grid.transform.rotation);
            
            Instantiate(grid, grid.transform.position + grid.transform.up * tilemapBounds.size.y, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + -grid.transform.up * tilemapBounds.size.y, grid.transform.rotation);
            
            Instantiate(grid, grid.transform.position + grid.transform.up * tilemapBounds.size.y + grid.transform.right * tilemapBounds.size.x, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + grid.transform.up * tilemapBounds.size.y + -grid.transform.right * tilemapBounds.size.x, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + -grid.transform.up * tilemapBounds.size.y + grid.transform.right * tilemapBounds.size.x, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + -grid.transform.up * tilemapBounds.size.y + -grid.transform.right * tilemapBounds.size.x, grid.transform.rotation);
        }
    }
}
