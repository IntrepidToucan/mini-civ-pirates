using UnityEngine;
using Utilities;

namespace Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/SettlementData", fileName = "SettlementData_")]
    public class SettlementData : ScriptableObject
    {
        public string inGameName = string.Empty;
        public OwningPlayer owner = OwningPlayer.None;
        
        public SettlementSize size = SettlementSize.Small;
    }
}
