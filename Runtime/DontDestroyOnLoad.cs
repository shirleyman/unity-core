using UnityEngine;

namespace OrbitalNine.Core
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            // Ensure this GameObject and its children are not destroyed when loading a new scene
            DontDestroyOnLoad(gameObject);
        }
    }
}
