using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "ScriptableObject/HarborData", fileName = "HarborData_")]
    public class HarborData : ScriptableObject
    {
        public SettlementData settlementData;
        public ShipData shipData;
    }
}
