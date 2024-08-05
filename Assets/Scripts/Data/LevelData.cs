using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/LevelData", fileName = "LevelData_")]
    public class LevelData : ScriptableObject
    {
        public List<TileData> startTilesData;
    }
}
