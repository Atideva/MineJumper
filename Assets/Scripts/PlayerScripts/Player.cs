using System;
using System.Collections;
using Configs;
using Gameplay;
using Jump;
using Parabola;
using Spawn;
using Unity.VisualScripting;
using UnityEngine;

namespace PlayerScripts
{
    public enum PlayerState
    {
        IsStand,
        IsJump,
        IsFall
    }

    public class Player : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] PlayerConfig config;
        [Header("Setup")]
        [SerializeField] PlayerJump jump;
        [SerializeField] PlayerSounds sounds;
        [SerializeField] PlayerLookAt lookAt;
        [SerializeField] PlayerSpeedBoost speedBoost;
        [SerializeField] PlayerJumpQuality jumpQuality;
        [SerializeField] PlayerStats stats;

        [Header("DEBUG")]
        [SerializeField] private PlayerState state;
        [Range(0, 1)] [SerializeField] private float debugSpeedFactor;
        [Range(0, 1)] [SerializeField] private float debugDangerFactor;

        public PlayerJump Jump => jump;
        Vector3 _offsetByY;
        int _currentCubeId;
        SpawnList _spawnList;

        public int Lives { get; private set; }

        #region Access

        public float LandThreshold => stats.LandThreshold;
        public float Speed => stats.Speed * (speedBoost.SpeedMult);
        public float JumpHeight => stats.JumpHeight;
        public int JumpStep => stats.JumpStep;
        public float DangerSpeedFactor => config.DangerSpeedFactor;
        public float SpeedBoostFactor => speedBoost.BoostValue;

        public int CurrentCubeId => _currentCubeId;

        #endregion


        private void Update()
        {
#if UNITY_EDITOR
            debugSpeedFactor = SpeedBoostFactor;
            debugDangerFactor = DangerSpeedFactor;
#endif
        }


        public event Action<float> OnSpeedBoostChanged = delegate { };
        public event Action<JumpQualityType> OnJump = delegate { };
        public event Action OnLiveLost = delegate { };
        public event Action OnLiveRestore = delegate { };
        public event Action OnRespawn = delegate { };

        void LiveLost()
        {
            Lives--;
            OnLiveLost();
        }

        void LiveRestore()
        {
            Lives++;
            OnLiveRestore();
        }

        public void Init(Vector3 startPos, float offsetY, SpawnList spawnList)
        {
            Lives = config.Lives;
            state = PlayerState.IsStand;
            _offsetByY = new Vector3(0, offsetY, 0);
            transform.position = startPos + _offsetByY;
            _spawnList = spawnList;

            stats.Init(config);
            jumpQuality.Init(stats);
            jump.Init(this);
            speedBoost.Init(config);

            Subscribe();
        }

        void Subscribe()
        {
            jump.OnLandAtCube += OnLandAtCube;
            jump.OnJumpFinish += OnJumpFinish;
        }

        void Respawn(Vector3 pos, Quaternion rotation)
        {
            transform.position = pos;
            transform.rotation = rotation;
            state = PlayerState.IsStand;
            OnRespawn();
        }

        void FallDown()
        {
            var startPos = transform.position;
            var dir = transform.forward;
            var fallDistance = 15;
            var fallHeight = 50;
            var endPos = startPos + dir * fallDistance + Vector3.down * fallHeight;
            var parabolaHeight = 0.3f;
            var spd = Speed / 5;
            var respawnPos = transform.position;
            var respawnRot = transform.rotation;
            StartCoroutine(FallAnimation(startPos, endPos, spd, parabolaHeight, respawnPos, respawnRot));
        }

        IEnumerator FallAnimation(Vector3 startPos, Vector3 endPos, float fallSpeed, float parabolaHeight,
            Vector3 respawnPos, Quaternion respawnRot)
        {
            var progress = 0f;
            while (progress < 1)
            {
                progress += fallSpeed * Time.deltaTime;
                transform.position = ParabolaMath.Parabola(startPos, endPos, parabolaHeight, progress);
                yield return null;
            }

            LiveLost();

            yield return new WaitForSeconds(2f);
            Respawn(respawnPos, respawnRot);
        }

        void OnJumpFinish(float jumpProgress)
        {
            var quality = JumpQualityType.First;

            if (jump.IsRow)
            {
                quality = jumpQuality.GetType(jump.jumpProgress);
            }

            if (quality == JumpQualityType.TooLate && SpeedBoostFactor >= DangerSpeedFactor)
            {
                state = PlayerState.IsFall;
                FallDown();
            }

            jumpQuality.Set(quality);
            UpdatePlayerSpeed(quality);
            OnJump(quality);
        }

     public   void OnJumpButton()
        {
            if (state == PlayerState.IsFall)
            {
                Debug.Log("You cannot jump when falling down");
                return;
            }

            if (jump.IsLand)
            {
                _currentCubeId += JumpStep;

                jump.StopLastJump();
                var pos = GetJumpPos(_currentCubeId);
                jump.NextJump(pos);

                state = PlayerState.IsJump;

                Debug.Log("Jump to cube: " + _currentCubeId);
            }
        }

        void OnLandAtCube()
        {
            sounds.PlayStone();
            var target = _spawnList.GetCube(_currentCubeId + lookAt.LookForwardBy).transform;
            lookAt.SetNewTarget(target);
        }

        void UpdatePlayerSpeed(JumpQualityType quality)
        {
            switch (quality)
            {
                case JumpQualityType.First:
                case JumpQualityType.BeforeTime:
                    speedBoost.Reset();
                    break;
                case JumpQualityType.Good:
                    break;
                case JumpQualityType.Perfect:
                    speedBoost.Increase();
                    break;
                case JumpQualityType.TooLate:
                    speedBoost.Reset();
                    break;
                default:
                    Debug.LogError("Missing JumpQualityType, sir");
                    break;
            }

            OnSpeedBoostChanged(speedBoost.BoostValue);
        }


        Vector3 GetJumpPos(int cubeId) => _spawnList.GetCube(cubeId).transform.position + _offsetByY;
    }
}