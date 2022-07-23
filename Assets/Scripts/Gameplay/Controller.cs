using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class Controller : MonoBehaviour
    {
        [SerializeField] private Button jumpButton;
        [SerializeField] KeyCode editorJumpKey;
        private Player _player;


        public void Init(Player player)
        {
            _player = player;
            jumpButton.onClick.AddListener(OnJumpButton);
        }

        void OnJumpButton()
        {
            _player.OnJumpButton();
        }

        private void Update()
        {
            if (Input.GetKeyDown(editorJumpKey))
            {
                OnJumpButton();
                Debug.Log("Jump pressed");
            }
        }
    }
}