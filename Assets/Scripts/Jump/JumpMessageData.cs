using System.Collections.Generic;
using UnityEngine;

namespace Jump
{
    [System.Serializable]
    public class JumpMessageData
    {
        public JumpQualityType qualityType;
        public List<string> content;
        public Color color;
    }
}