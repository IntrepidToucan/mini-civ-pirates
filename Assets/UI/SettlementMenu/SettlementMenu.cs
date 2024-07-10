using Settlements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.SettlementMenu
{
    public class SettlementMenu : MonoBehaviour
    {
        [Header("UXML")]
        [SerializeField] private VisualTreeAsset harborListItemUxml;

        private UIDocument _uiDoc;
        private VisualElement _harborsList;
        
        private Settlement _settlement;

        public void SetParams(Settlement settlement)
        {
            _settlement = settlement;
            
            _harborsList.Clear();
            
            foreach (var harbor in _settlement.Harbors)
            {
                var listItem = harborListItemUxml.Instantiate();
                
                var buildShipButton = listItem.Q("build-ship-button");
                buildShipButton.RegisterCallback<ClickEvent, Settlement.HarborData>(BuildShip, harbor);
                
                _harborsList.Add(listItem);
            }
        }

        private void Awake()
        {
            _uiDoc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            var closeButton = _uiDoc.rootVisualElement.Q<Button>();
            closeButton.RegisterCallback<ClickEvent>(CloseMenu);
            
            _harborsList = _uiDoc.rootVisualElement.Q("harbors-list");
        }

        private static void CloseMenu(ClickEvent evt)
        {
            Player.Player.Instance.UiController.HideSettlementMenu();
        }

        private static void BuildShip(ClickEvent evt, Settlement.HarborData harbor)
        {
            Player.Player.Instance.ResourceController.BuildShip(new Vector2());
            Player.Player.Instance.UiController.HideSettlementMenu();
        }
    }
}
