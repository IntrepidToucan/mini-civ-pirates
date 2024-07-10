using Settlements;
using UI.SettlementMenu;
using UnityEngine;

namespace Player
{
    public class PlayerUiController : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject settlementMenuPrefab;

        private SettlementMenu _settlementMenu;
        
        public void ShowSettlementMenu(Settlement settlement)
        {
            HideSettlementMenu();

            _settlementMenu = Instantiate(settlementMenuPrefab.gameObject).GetComponent<SettlementMenu>();
            _settlementMenu.SetParams(settlement);
        }

        public void HideSettlementMenu()
        {
            if (_settlementMenu is null) return;
            
            Destroy(_settlementMenu.gameObject);
            _settlementMenu = null;
        }
    }
}
