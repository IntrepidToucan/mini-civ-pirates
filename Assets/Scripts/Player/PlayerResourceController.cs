using UnityEngine;

namespace Player
{
    public class PlayerResourceController : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject shipPrefab;

        public void BuildShip(Vector2 position)
        {
            Instantiate(shipPrefab, position, Quaternion.identity);
        }
    }
}
