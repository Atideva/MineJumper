using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Game config", menuName = "Game/_GAME Config")]
    public class GameConfig : ScriptableObject
    {
        [Header("ALL CONFIGS")]
        [SerializeField] private MessagesConfig messages;

        public MessagesConfig Messages => messages;
 
    }
}