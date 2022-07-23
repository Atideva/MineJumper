using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;

namespace UI
{
    public class LivesUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> livesIcons = new();
 
        private int _lives;
        
        public void Init(Player player)
        {
            _lives = player.Lives;
            player.OnLiveLost += Lost;
            player.OnLiveRestore += Restore;
            Refresh();
        }

        void Lost()
        {
            _lives--;
            Refresh();
        }

        void Restore()
        {
            _lives++;
            Refresh();
        }
        void Refresh()
        {
            for (var i = 0; i < livesIcons.Count; i++)
            {
                livesIcons[i].gameObject.SetActive(i < _lives);
            }
        }
    }
}
