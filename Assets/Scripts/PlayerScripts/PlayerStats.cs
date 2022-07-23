using Configs;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerStats : MonoBehaviour
    {
        public float Speed => _speed;
        public int JumpStep => _jumpStep;
        public float JumpHeight => _jumpHeight;
        public float LandThreshold => _landThreshold;
        public float PerfectThreshold => _perfectThreshold;

        float _landThreshold;
        float _perfectThreshold;
        float _speed;
        float _jumpHeight;
        int _jumpStep;

        PlayerConfig _config;

        public void Init(PlayerConfig config)
        {
            _config = config;
            _speed = config.BaseSpeed;
            _jumpHeight = config.JumpHeight;
            _jumpStep = config.JumpStep;
            _landThreshold = config.LandThreshold;
            _perfectThreshold = config.PerfectThreshold;
        }
    }
}