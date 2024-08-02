using Cinemachine;
using UnityEngine;
using Utilities;

namespace Cameras
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CinemachineBrain))]
    public class MainCamera : PersistedSingleton<MainCamera>
    {
        public Camera Camera { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();

            Camera = GetComponent<Camera>();
            Camera.orthographic = true;
        }
    }
}
