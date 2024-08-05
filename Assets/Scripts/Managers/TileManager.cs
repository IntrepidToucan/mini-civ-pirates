using System;
using System.Collections.Generic;
using System.Linq;
using Cameras;
using Data;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;
using TileData = Data.TileData;
using Vector3Int = UnityEngine.Vector3Int;

namespace Managers
{
    public class TileManager : PersistedSingleton<TileManager>
    {
        [Header("Data")]
        [SerializeField] private LevelData levelData;
        
        [Header("Debug")]
        [SerializeField] private bool debugTiles;
        
        private Tilemap _landTilemap;
        private Tilemap _actorsTilemap;

        private Dictionary<Vector3Int, TileData> _tileDataDict;
        
        public Tilemap WaterTilemap { get; private set; }

        public void HandleMousePosition(Vector2 mousePosition)
        {
        }

        public void HandleSelect(Vector2 mousePosition)
        {
            var worldPosition = MainCamera.Instance.Camera.ScreenToWorldPoint(mousePosition);
            var cellPosition = WaterTilemap.layoutGrid.LocalToCell(worldPosition);
            
            Debug.Log(cellPosition);
        }

        protected override void Awake()
        {
            base.Awake();
            
            PopulateTilemapVars();
            PopulateTileDataDict();
            CreateGhostGrids();
        }
        
        private void PopulateTilemapVars()
        {
            WaterTilemap = GameObject.Find("/Grid/Water").GetComponent<Tilemap>();
            _landTilemap = GameObject.Find("/Grid/Land").GetComponent<Tilemap>();
            _actorsTilemap = GameObject.Find("/Grid/Actors").GetComponent<Tilemap>();

            foreach (var tilemap in new[] { WaterTilemap, _landTilemap, _actorsTilemap })
            {
                tilemap.CompressBounds();
            }
        }

        private void PopulateTileDataDict()
        {
            _tileDataDict = new Dictionary<Vector3Int, TileData>();

            foreach (var pos in WaterTilemap.cellBounds.allPositionsWithin)
            {
                if (debugTiles)
                {
                    var debugStr = $"pos: {pos}";
                    if (WaterTilemap.HasTile(pos)) debugStr += " | water";
                    if (_landTilemap.HasTile(pos)) debugStr += " | land";
                    if (_actorsTilemap.HasTile(pos)) debugStr += " | actor";
                    
                    Debug.Log(debugStr);
                }

                var tile = (from tilemap in new[] { _actorsTilemap, _landTilemap, WaterTilemap }
                    where tilemap.HasTile(pos) select tilemap.GetTile(pos)).FirstOrDefault();

                if (tile != null)
                {
                    var tileData = levelData.startTilesData.Find(data => data.tiles.Contains(tile));

                    if (tileData != null)
                    {
                        var clonedTileData = Instantiate(tileData);
                        _tileDataDict.Add(pos, clonedTileData);
                        HandleTile(pos, clonedTileData);
                    }
                    else
                    {
                        Debug.LogError($"No tile data found for tile {tile.name} at position {pos}");
                    }
                }
                else
                {
                    Debug.LogError($"No tile found for position {pos}");
                }
            }
        }

        private void HandleTile(Vector3Int pos, TileData data)
        {
            switch (data.tileType)
            {
                case TileType.Harbor:
                    var shipData = data.harborData.shipData;
                    
                    if (shipData != null)
                    {
                        Instantiate(ResourceManager.Instance.ShipPrefab, WaterTilemap.GetCellCenterWorld(pos), Quaternion.identity);
                    }

                    break;
                case TileType.Settlement:
                    break;
                case TileType.Land:
                case TileType.Water:
                case TileType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
