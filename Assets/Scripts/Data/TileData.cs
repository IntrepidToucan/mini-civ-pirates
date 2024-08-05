using System;
using System.Collections.Generic;
using System.Linq;
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

        [Header("Type Metadata - Populate no more than ONE")]
        public SettlementData settlementData;
        public HarborData harborData;
        
        [Header("Runtime")]
        public bool isExplored;
        
        private void Awake()
        {
            var dataCount = new ScriptableObject[] { harborData, settlementData }.Count(data => data != null);

            if (dataCount > 1)
            {
                Debug.LogError($"Tile data {name} has {dataCount} data objects");
            }

            switch (tileType)
            {
                case TileType.Harbor:
                    if (harborData == null)
                    {
                        Debug.LogError($"TileData {name} is missing harbor data");
                    }
                    
                    break;
                case TileType.Settlement:
                    if (settlementData == null)
                    {
                        Debug.LogError($"TileData {name} is missing settlement data");
                    }
                    
                    break;
                case TileType.Land:
                case TileType.Water:
                case TileType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
