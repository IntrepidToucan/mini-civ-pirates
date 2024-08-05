using UnityEngine;
using Utilities;

namespace Managers
{
    public class ResourceManager : PersistedSingleton<ResourceManager>
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject shipPrefab;

        public GameObject ShipPrefab => shipPrefab;
    }
}
