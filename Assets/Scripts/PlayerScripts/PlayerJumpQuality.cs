using System.Collections.Generic;
using Jump;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerJumpQuality : MonoBehaviour
    {
        [field: SerializeField, Header("DEBUG")]
        public JumpQualityType LastJumpQuality { get; private set; }

        public List<JumpQualityType> allJumps = new();

        private PlayerStats _stats;

        public void Init(PlayerStats stats)
        {
            _stats = stats;
        }

        public void Set(JumpQualityType lastType)
        {
            LastJumpQuality = lastType;
#if UNITY_EDITOR
            allJumps.Add(lastType);
#endif
        }

    


        public JumpQualityType GetType(float jumpProgress)
        {
            JumpQualityType result;

            var gap = Mathf.Abs(1 - jumpProgress);
            if (gap > _stats.LandThreshold)
            {
                result = jumpProgress < 1
                    ? JumpQualityType.BeforeTime
                    : JumpQualityType.TooLate;
            }
            else
            {
                result = gap <= _stats.PerfectThreshold
                    ? JumpQualityType.Perfect
                    : JumpQualityType.Good;
            }

            return result;
        }
    }
}