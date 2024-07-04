using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Utilities;

namespace Player
{
    [RequireComponent(typeof(InputController))]
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(PlayerInput))]
    public class Player : Singleton<Player>
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject eventSystemPrefab;
        [SerializeField] private GameObject virtualCameraPrefab;
        
        public MovementController MovementController { get; private set; }
        public PlayerInput PlayerInput { get; private set; }
        public Tilemap WaterTilemap { get; private set; }

        protected override void Awake()
        {
            persistAcrossScenes = true;
            
            base.Awake();
        
            Instantiate(eventSystemPrefab);
            Instantiate(virtualCameraPrefab);

            MovementController = GetComponent<MovementController>();
            PlayerInput = GetComponent<PlayerInput>();
        }

        private void Start()
        {
            WaterTilemap = GameObject.Find("/Grid/Water").GetComponent<Tilemap>();
        }
    }
}
