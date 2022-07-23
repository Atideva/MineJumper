using System.Collections.Generic;
using System.Linq;
using Jump;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Messages config", menuName = "Game/Messages Config")]
    public class MessagesConfig : ScriptableObject
    {
 
        [Header("Config")]
        public List<JumpMessageData> jumpQuality = new();

 

        public JumpMessageData GetJumpData(JumpQualityType qualityType)
        {
            var jumpData = jumpQuality.FirstOrDefault(q => q.qualityType == qualityType);
            if (jumpData != null) return jumpData;
            
            Debug.LogError("There's no such data, sir");
            return null;

        }

 
    }
}