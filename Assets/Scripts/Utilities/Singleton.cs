using UnityEngine;

namespace Utilities
{
    /**
     * Adapted from
     * https://gitlab.com/GameDevTV/unity-2d-rpg-combat/unity-2d-rpg-combat-course/-/commit/468b18447d24f633447432c6ed0fcf963edcf45c#d4610e20b303592b371bbe2fa044e6b6ffa17af5.
     */
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        [SerializeField] protected bool persistAcrossScenes;
        
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != this && Instance != null)
            {
                if (gameObject != null) Destroy(gameObject);
            }
            else
            {
                Instance = (T)this;
                
                if (persistAcrossScenes) DontDestroyOnLoad(gameObject);
            }
        }
    }
}
