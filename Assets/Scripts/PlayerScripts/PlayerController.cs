using System;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] KeyCode editorJumpKey;
        public event Action OnJumpButton = delegate { };

        private void Update()
        {
 
            if (Input.GetKeyDown(editorJumpKey))
            {
                Debug.Log("Jump pressed");
                OnJumpButton();
            }
 
        }
    }
}