using System.Collections.Generic;
using Cameras;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;

namespace Managers
{
    public class TileManager : PersistedSingleton<TileManager>
    {
        [Header("Debug")]
        [SerializeField] private bool debugUtils;
        
        [Header("Tiles")]
        [SerializeField] private TileBase fogOfWarTile;
        
        public Tilemap WaterTilemap { get; private set; }

        private List<Grid> _ghostGrids;
        private Tilemap _fogOfWarTilemap;

        public List<Vector3> GetGhostGridPositions(Vector3 worldPos)
        {
            var tilemapSize = WaterTilemap.cellBounds.size;
            var grid = WaterTilemap.layoutGrid;
            
            return new List<Vector3>
            {
                // Horizontal grids
                worldPos + grid.transform.right * tilemapSize.x,
                worldPos + -grid.transform.right * tilemapSize.x,
                // Vertical grids
                worldPos + grid.transform.up * tilemapSize.y,
                worldPos + -grid.transform.up * tilemapSize.y,
                // Diagonal grids
                worldPos + grid.transform.up * tilemapSize.y + grid.transform.right * tilemapSize.x,
                worldPos + grid.transform.up * tilemapSize.y + -grid.transform.right * tilemapSize.x,
                worldPos + -grid.transform.up * tilemapSize.y + grid.transform.right * tilemapSize.x,
                worldPos + -grid.transform.up * tilemapSize.y + -grid.transform.right * tilemapSize.x
            };
        }

        public Vector3Int GetCellPositionForMouse(Vector3 mousePos)
        {
            return WaterTilemap.WorldToCell(GetNormalizedWorldPositionForMouse(mousePos));
        }

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
                if (!_fogOfWarTilemap.HasTile(pos)) continue;
                
                _fogOfWarTilemap.SetTile(pos, null);
                
                var ghostPositions = GetGhostGridPositions(_fogOfWarTilemap.GetCellCenterWorld(pos));

                for (var i = 0; i < _ghostGrids.Count; i++)
                {
                    var tilemap = _ghostGrids[i].transform.Find("FogOfWar").GetComponent<Tilemap>();
                    tilemap.SetTile(tilemap.WorldToCell(ghostPositions[i]), null);
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _ghostGrids = new List<Grid>();
            
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
            var grid = WaterTilemap.layoutGrid;

            foreach (var pos in GetGhostGridPositions(grid.transform.position))
            {
                _ghostGrids.Add(Instantiate(grid, pos, grid.transform.rotation));
            }
        }
        
        private Vector3 GetNormalizedWorldPositionForMouse(Vector3 mousePos)
        {
            var rawWorldPos = MainCamera.Instance.Camera.ScreenToWorldPoint(mousePos);
            var result = new Vector3(rawWorldPos.x, rawWorldPos.y, 0f);
            var tilemapBounds = WaterTilemap.cellBounds;

            if (result.x < tilemapBounds.xMin)
            {
                result.x += tilemapBounds.size.x;
            } else if (result.x > tilemapBounds.xMax)
            {
                result.x -= tilemapBounds.size.x;
            }
            
            if (result.y < tilemapBounds.yMin)
            {
                result.y += tilemapBounds.size.y;
            } else if (result.y > tilemapBounds.yMax)
            {
                result.y -= tilemapBounds.size.y;
            }

            if (debugUtils)
            {
                Debug.Log($"World pos raw: {rawWorldPos}, normalized: {result}");
            }
            
            return result;
        }
    }
}
