using Ships;
using UI.ShipMenu;
using UnityEngine;
using UnityEngine.UIElements;
using Utilities;

namespace Managers
{
    public class UiManager : PersistedSingleton<UiManager>
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject shipMenuPrefab;

        private ShipMenu _shipMenu;

        private void OnEnable()
        {
            EventManager.OnActiveObjectChange += HandleActiveObjectChange;
        }

        private void OnDisable()
        {
            EventManager.OnActiveObjectChange -= HandleActiveObjectChange;
        }

        private void HandleActiveObjectChange(GameObject newActiveObject)
        {
            if (_shipMenu != null) Destroy(_shipMenu.gameObject);
            
            if (newActiveObject.TryGetComponent<Ship>(out var ship))
            {
                _shipMenu = Instantiate(shipMenuPrefab).GetComponent<ShipMenu>();
                _shipMenu.SetShip(ship);
            }
        }
    }
}
