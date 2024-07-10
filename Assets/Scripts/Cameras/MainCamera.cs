using Cinemachine;
using UnityEngine;
using Utilities;

namespace Cameras
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(CinemachineBrain))]
    public class MainCamera : Singleton<MainCamera>
    {
        public Camera Camera { get; private set; }
        
        protected override void Awake()
        {
            persistAcrossScenes = true;
            
            base.Awake();

            Camera = GetComponent<Camera>();
            Camera.orthographic = true;
        }
    }
}
