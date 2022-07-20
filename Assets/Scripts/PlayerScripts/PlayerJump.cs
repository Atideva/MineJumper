using System;
using Parabola;
using UnityEngine;

namespace PlayerScripts
{
    [ExecuteInEditMode]
    public class PlayerJump : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float jumpHeight;
        [SerializeField] float jumpDuration;
        [Range(0f, 0.3f)]
        float _landThreshold;
        [SerializeField] private Transform player;

        Vector3 _startPos, _endPos;
        private float _jumpDuration, _jumpTime;

        public event Action OnLandAtCube = delegate { };
        public event Action<float> OnJump = delegate { };

        [field: SerializeField] public bool IsJumping { get; private set; }
        bool _isLanding;
        public bool NextJumpAllowed => _isLanding;
        public bool JumpInProcess => IsJumping && !NextJumpAllowed;


        [Header("Debug")]
        [Range(0, 1f)] public float jumpProgress;


        public void Init(float landThreshold)
        {
            _landThreshold = landThreshold;
        }

        private void Update()
        {
            if (IsJumping)
            {
                _jumpTime += Time.deltaTime;
                jumpProgress = _jumpTime / _jumpDuration;

                if (jumpProgress >= 1 + _landThreshold)
                {
                    IsJumping = false;
                }

                CheckIsLand();
                Move(jumpProgress);
            }
        }

        void CheckIsLand()
        {
            if (_isLanding) return;

            var land = 1f - _landThreshold;

            if (jumpProgress >= land)
            {
                _isLanding = true;
                OnLandAtCube();
            }
        }

        public void Jump(Vector3 targetPos, float spdMult)
        {
            OnJump(jumpProgress);

            IsJumping = true;
            _isLanding = false;

            if (spdMult <= 0)
            {
                spdMult = 0.1f;
                Debug.LogError("Your speed multiplier is below 0. It this legal?");
            }

            _jumpTime = 0;
            _jumpDuration = jumpDuration / spdMult;
            _startPos = player.position;
            _endPos = targetPos;
        }



        void Move(float progress)
        {
            progress = Mathf.Clamp01(progress);
            player.position = MathParabola.Parabola(_startPos, _endPos, jumpHeight, progress);
        }
    }
}