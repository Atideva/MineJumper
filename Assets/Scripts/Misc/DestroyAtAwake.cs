using UnityEngine;

namespace Misc
{
    public class DestroyAtAwake : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}