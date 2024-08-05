using UnityEngine;
using Utilities;

namespace Managers
{
    public class GameManager : PersistedSingleton<GameManager>
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject eventSystemPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject resourceManagerPrefab;
        [SerializeField] private GameObject tileManagerPrefab;
        [SerializeField] private GameObject uiManagerPrefab;

        protected override void Awake()
        {
            base.Awake();
            
            Instantiate(eventSystemPrefab);
            Instantiate(playerPrefab);
            Instantiate(resourceManagerPrefab);
            Instantiate(tileManagerPrefab);
            Instantiate(uiManagerPrefab);
        }
    }
}
