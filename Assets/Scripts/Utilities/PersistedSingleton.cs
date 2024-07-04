namespace Utilities
{
    public class PersistedSingleton : Singleton<PersistedSingleton>
    {
        protected override void Awake()
        {
            persistAcrossScenes = true;
            
            base.Awake();
        }
    }
}
