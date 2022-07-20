using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Messages config", menuName = "Game/Messages Config")]
public class MessagesConfig : ScriptableObject
{
 
    [Header("Config")]
    public List<JumpMessageData> jumpQuality = new();

    public IReadOnlyList<JumpMessageData> JumpQuality => jumpQuality;

    public JumpMessageData GetJumpData(JumpQualityType qualityType)
    {
        var jumpData = jumpQuality.FirstOrDefault(q => q.qualityType == qualityType);
        if (jumpData == null)
        {
            Debug.LogError("There's no such data, sir");
            return null;
        }

        return jumpData;
    }

 
}