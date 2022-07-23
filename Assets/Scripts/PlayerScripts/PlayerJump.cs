using System;
using Parabola;
using UnityEngine;

namespace PlayerScripts
{
    [ExecuteInEditMode]
    public class PlayerJump : MonoBehaviour
    {
        [Header("Debug")]
        [Range(0, 1f)] public float jumpProgress;
        [field: SerializeField] public bool IsJumping { get; private set; }
        [field: SerializeField] public bool IsRow { get; private set; }
        public bool IsLand { get; private set; }

        Player _player;
        Vector3 _startPos, _endPos;
        float _curJumpHeight;
        public event Action OnLandAtCube = delegate { };
        public event Action<float> OnJumpFinish = delegate { };


        public void Init(Player player)
        {
            _player = player;
            IsLand = true;
        }

        void Update()
        {
            if (IsJumping)
            {
                jumpProgress += Time.deltaTime * _player.Speed;

                if (jumpProgress >= 1 + _player.LandThreshold)
                {
                    Stop();
                }

                CheckLand();
                Move(jumpProgress);
            }
        }

        void CheckLand()
        {
            if (IsLand) return;

            var land = 1f - _player.LandThreshold;

            if (jumpProgress >= land)
            {
                IsLand = true;
                OnLandAtCube();
            }
        }

        void Stop()
        {
            StopLastJump();
            IsRow = false;
            IsJumping = false;
        }

        public void StopLastJump()
        {
            if (IsJumping)
                OnJumpFinish(jumpProgress);
        }

        public void NextJump(Vector3 targetPos)
        {
            if (IsJumping)
            {
                IsRow = true;
            }

            IsJumping = true;
            IsLand = false;

            jumpProgress = 0;
            _startPos = _player.transform.position;
            _endPos = targetPos;
            _curJumpHeight = _player.JumpHeight;
        }


        void Move(float progress)
        {
            progress = Mathf.Clamp01(progress);
            _player.transform.position = ParabolaMath.Parabola(_startPos, _endPos, _curJumpHeight, progress);
        }
    }
}