using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utilities;

namespace Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/TileData", fileName = "TileData_")]
    public class TileData : ScriptableObject
    {
        [Header("Tile Metadata")]
        public TileType tileType = TileType.None;
        public List<TileBase> tiles;
    }
}
