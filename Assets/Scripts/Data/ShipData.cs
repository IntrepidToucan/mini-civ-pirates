using UnityEngine;
using Utilities;

namespace Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/ShipData", fileName = "ShipData_")]
    public class ShipData : ScriptableObject
    {
        public string inGameName = string.Empty;
        public OwningPlayer owner = OwningPlayer.None;
        
        public ShipSize size = ShipSize.Small;
    }
}
