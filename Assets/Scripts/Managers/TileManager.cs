using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;

namespace Managers
{
    public class TileManager : PersistedSingleton<TileManager>
    {
        [Header("Tiles")]
        [SerializeField] private TileBase fogOfWarTile;
        
        public Tilemap WaterTilemap { get; private set; }

        private Tilemap _fogOfWarTilemap;

        public void ExploreWorldPosition(Vector3 worldPos)
        {
            var cellPos = _fogOfWarTilemap.WorldToCell(worldPos);

            foreach (var pos in new[]
                     {
                         cellPos,
                         cellPos + Vector3Int.up,
                         cellPos + Vector3Int.down,
                         cellPos + Vector3Int.left,
                         cellPos + Vector3Int.right,
                         cellPos + Vector3Int.up + Vector3Int.left,
                         cellPos + Vector3Int.up + Vector3Int.right,
                         cellPos + Vector3Int.down + Vector3Int.left,
                         cellPos + Vector3Int.down + Vector3Int.right,
                         cellPos + Vector3Int.up * 2,
                         cellPos + Vector3Int.down * 2,
                         cellPos + Vector3Int.left * 2,
                         cellPos + Vector3Int.right * 2,
                     })
            {
                _fogOfWarTilemap.SetTile(pos, null);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
            PopulateTilemapVars();
            PopulateTileData();
            CreateGhostGrids();
        }

        private void PopulateTilemapVars()
        {
            WaterTilemap = GameObject.Find("/Grid/Water").GetComponent<Tilemap>();
            
            _fogOfWarTilemap = GameObject.Find("/Grid/FogOfWar").GetComponent<Tilemap>();
            _fogOfWarTilemap.GetComponent<TilemapRenderer>().sortingLayerID = SortingLayer.NameToID("FogOfWar");

            foreach (var tilemap in new[] { WaterTilemap, _fogOfWarTilemap })
            {
                tilemap.CompressBounds();
            }
        }
        
        private void PopulateTileData()
        {
            foreach (var cellPos in WaterTilemap.cellBounds.allPositionsWithin)
            {
                // NOTE: We currently have a hack in place where the "FogOfWar" sorting layer has no lighting
                // so that we can use a fully opaque tile from our prototype art set and have it appear as black.
                _fogOfWarTilemap.SetTile(cellPos, fogOfWarTile);
            }
        }

        private void CreateGhostGrids() {
            var tilemapSize = WaterTilemap.cellBounds.size;
            var grid = WaterTilemap.layoutGrid;
            
            // Create horizontal grids.
            Instantiate(grid, grid.transform.position + grid.transform.right * tilemapSize.x, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + -grid.transform.right * tilemapSize.x, grid.transform.rotation);
            
            // Create vertical grids.
            Instantiate(grid, grid.transform.position + grid.transform.up * tilemapSize.y, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + -grid.transform.up * tilemapSize.y, grid.transform.rotation);
            
            // Create diagonal grids.
            Instantiate(grid, grid.transform.position + grid.transform.up * tilemapSize.y + grid.transform.right * tilemapSize.x, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + grid.transform.up * tilemapSize.y + -grid.transform.right * tilemapSize.x, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + -grid.transform.up * tilemapSize.y + grid.transform.right * tilemapSize.x, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + -grid.transform.up * tilemapSize.y + -grid.transform.right * tilemapSize.x, grid.transform.rotation);
        }
    }
}
