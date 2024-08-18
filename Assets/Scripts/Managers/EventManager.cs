using System;
using Ships;
using UnityEngine;

namespace Managers
{
    public static class EventManager
    {
        public static event Action<GameObject> OnActiveObjectChange;
        public static event Action<Ship, int> OnShipFoodChange;

        public static void TriggerActiveObjectChange(GameObject newActiveObject) =>
            OnActiveObjectChange?.Invoke(newActiveObject);
        public static void TriggerShipFoodChange(Ship ship, int foodCount) =>
            OnShipFoodChange?.Invoke(ship, foodCount);
    }
}
