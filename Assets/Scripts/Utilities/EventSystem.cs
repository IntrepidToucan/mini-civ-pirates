using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace Utilities
{
    [RequireComponent(typeof(UnityEngine.EventSystems.EventSystem))]
    [RequireComponent(typeof(InputSystemUIInputModule))]
    public class EventSystem : PersistedSingleton<EventSystem>
    {
    }
}
