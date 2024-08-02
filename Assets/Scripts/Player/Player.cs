using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerMovementController))]
    public class Player : PersistedSingleton<Player>
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject playerFollowCameraPrefab;
        
        public PlayerInput PlayerInput { get; private set; }
        public PlayerMovementController MovementController { get; private set; }

        protected override void Awake()
        {
            base.Awake();
        
            Instantiate(playerFollowCameraPrefab);

            MovementController = GetComponent<PlayerMovementController>();
            PlayerInput = GetComponent<PlayerInput>();
        }
    }
}
