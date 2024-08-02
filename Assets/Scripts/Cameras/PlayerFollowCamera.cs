using Cinemachine;
using UnityEngine;
using Utilities;

namespace Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PlayerFollowCamera : PersistedSingleton<PlayerFollowCamera>
    {
        public CinemachineVirtualCamera CineVirtualCamera { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();

            CineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            CineVirtualCamera.m_Lens.OrthographicSize = 7f;
            CineVirtualCamera.m_Lens.NearClipPlane = 0f;
            CineVirtualCamera.m_Lens.FarClipPlane = 50f;
        }

        private void Start()
        {
            CineVirtualCamera.Follow = Player.Player.Instance.transform;
        }
    }
}
