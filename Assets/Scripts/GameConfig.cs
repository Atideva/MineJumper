 
using UnityEngine;

[CreateAssetMenu(fileName = "Game config", menuName = "Game/_GAME Config")]
public class GameConfig : ScriptableObject
{
    [Header("ALL CONFIGS")]
    [SerializeField] private MessagesConfig messages;
    [SerializeField] private JumpConfig jump;

    public MessagesConfig Messages => messages;

    public JumpConfig Jump => jump;
}