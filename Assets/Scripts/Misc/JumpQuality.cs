using UnityEngine;

public class JumpQuality : MonoBehaviour
{
    private float _land;
    private float _perfect;

    public void Init(JumpConfig data)
    {
        _land = data.LandThreshold;
        _perfect = data.PerfectThreshold;
    }

    public JumpQualityType GetQuality(float jumpProgress)
    {
        var gap = Mathf.Abs(1 - jumpProgress);

        if (gap > _land)
        {
            return jumpProgress < 1
                ? JumpQualityType.BeforeTime
                : JumpQualityType.TooLate;
        }

        return gap <= _perfect
            ? JumpQualityType.Perfect
            : JumpQualityType.Good;
    }
}