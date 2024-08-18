using System;
using System.Globalization;
using Managers;
using Ships;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.ShipMenu
{
    public class ShipMenu : MonoBehaviour
    {
        private Ship _ship;
        private UIDocument _uiDoc;
        private Label _title;
        private Label _foodCount;
        
        public void SetShip(Ship ship)
        {
            _ship = ship;
            _title.text = ship.GetInGameName();
            _foodCount.text = ship.GetFood().ToString();
        }

        private void Awake()
        {
            _uiDoc = GetComponent<UIDocument>();
            _title = _uiDoc.rootVisualElement.Q<Label>("ship-menu-title");
            _foodCount = _uiDoc.rootVisualElement.Q<Label>("ship-menu-food-count");
        }

        private void OnEnable()
        {
            EventManager.OnShipFoodChange += HandleShipFoodChange;
        }

        private void OnDisable()
        {
            EventManager.OnShipFoodChange -= HandleShipFoodChange;
        }

        private void HandleShipFoodChange(Ship ship, int foodCount)
        {
            if (_ship != ship) return;

            _foodCount.text = string.Format(CultureInfo.CurrentCulture, "{0:N0}", foodCount);
        }
    }
}
