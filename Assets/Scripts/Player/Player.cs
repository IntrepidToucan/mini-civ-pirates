using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Utilities;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerInteractionController))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerResourceController))]
    [RequireComponent(typeof(PlayerUiController))]
    public class Player : Singleton<Player>
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject eventSystemPrefab;
        [SerializeField] private GameObject virtualCameraPrefab;
        
        public PlayerInput PlayerInput { get; private set; }
        public PlayerInteractionController InteractionController { get; private set; }
        public PlayerMovementController MovementController { get; private set; }
        public PlayerResourceController ResourceController { get; private set; }
        public PlayerUiController UiController { get; private set; }
        public Tilemap WaterTilemap { get; private set; }

        protected override void Awake()
        {
            persistAcrossScenes = true;
            
            base.Awake();
        
            Instantiate(eventSystemPrefab);
            Instantiate(virtualCameraPrefab);

            InteractionController = GetComponent<PlayerInteractionController>();
            MovementController = GetComponent<PlayerMovementController>();
            ResourceController = GetComponent<PlayerResourceController>();
            PlayerInput = GetComponent<PlayerInput>();
            UiController = GetComponent<PlayerUiController>();
        }

        private void Start()
        {
            WaterTilemap = GameObject.Find("/Grid/Water").GetComponent<Tilemap>();

            var grid = GameObject.Find("/Grid");
            Instantiate(grid, grid.transform.position + Vector3.right * 30f, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + Vector3.left * 30f, grid.transform.rotation);
            
            Instantiate(grid, grid.transform.position + Vector3.up * 40f, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + Vector3.down * 40f, grid.transform.rotation);
            
            Instantiate(grid, grid.transform.position + Vector3.up * 40f + Vector3.right * 30f, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + Vector3.up * 40f + Vector3.left * 30f, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + Vector3.down * 40f + Vector3.right * 30f, grid.transform.rotation);
            Instantiate(grid, grid.transform.position + Vector3.down * 40f + Vector3.left * 30f, grid.transform.rotation);
        }
    }
}
