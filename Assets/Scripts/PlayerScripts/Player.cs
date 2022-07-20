using Gameplay;
using UnityEngine;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        [SerializeField] Vector3 standOffsetByY;
        [SerializeField] int jumpStep;
        [SerializeField] PlayerJump jump;
        [SerializeField] PlayerSounds sounds;
        [SerializeField] PlayerController controller;
        [SerializeField] PlayerLookAt lookAt;
        [SerializeField] PlayerSpeedBoost speedBoost;


        private int _nextCubeId;
        private CubeList _cubeList;
        public PlayerJump Jump => jump;
        public float SpeedBoostFactor => speedBoost.SpeedFactor;
        public void Init(CubeList cubeList, Vector3 startPos, float jumpLandThreshold)
        {
            _cubeList = cubeList;

            transform.position = startPos + standOffsetByY;

            jump.Init(jumpLandThreshold);
            jump.OnLandAtCube += OnLandAtCube;
            
            controller.OnJumpButton += OnJumpButton;

        //    jump.OnJump += OnJump;
        }


        private void OnJumpButton()
        {
            if (jump.JumpInProcess)
            {
                return;
            }

            _nextCubeId += jumpStep;
            Debug.Log("Jump to: " + _nextCubeId);

            var pos = _cubeList.GetCube(_nextCubeId).transform.position + standOffsetByY;
            jump.Jump(pos, speedBoost.SpeedMult);
        }

        void OnLandAtCube()
        {
            sounds.PlayStone();
            var target = _cubeList.GetCube(_nextCubeId + lookAt.LookForwardBy).transform;
            lookAt.SetNewTarget(target);
        }

 
        // void OnJump(JumpQualityType jumpQualityType)
        // {
        //     if (jumpQualityType == JumpQualityType.Bad)
        //     {
        //         speedBoost.ResetBoost();
        //     }
        //
        //     if (jumpQualityType == JumpQualityType.Perfect)
        //     {
        //         speedBoost.IncreaseBoost();
        //     }
        // }
    }
}