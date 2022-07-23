using Configs;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerSpeedBoost : MonoBehaviour
    {
 
    
        [Header("DEBUG")]
        [SerializeField] float _speedMult;
        [SerializeField] int _currentRow;
          PlayerConfig _config;

        public void Init(PlayerConfig config)
        {
            _config = config;
            _speedMult = 1;
        }
 
        public float SpeedMult => _speedMult;
        public float BoostValue => _config.JumpsToMaxSpeed > 0 ? (float) _currentRow / _config.JumpsToMaxSpeed : 0;
        public void Increase()
        {
            _currentRow++;

            var bonus = (_config.SpeedBoostFactor - 1) * BoostValue;
            _speedMult = 1 + bonus;
        }

        public void Reset()
        {
            _currentRow = 0;
            _speedMult = 1;
        }
    }
}