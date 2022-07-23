using UnityEngine;

/*

                -landThreshold                 LandAtCube                     +landThreshold
                    |------------------------------|------------------------------|
                    
                    
_________________| |______________________| |____________| |_____________________| |_________________________
       BeforeTime            Good              Perfect             Good             TooLate   
                           
*/


namespace Configs
{
    [CreateAssetMenu(fileName = "Player", menuName = "Game/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Lives")]
        [SerializeField] private int lives=3;
        
        [Header("Speed")]
        [SerializeField] float baseSpeed = 1f;
        [Range(0, 1)]
        [SerializeField] float dangerSpeedFactor = 0.5f;

        [Header("Speed Boost")]
        [SerializeField] [Range(1, 5)] float speedBoostFactor = 1;
        [SerializeField] int jumpsToMaxSpeed;
        
        [Header("Jump")]
        [SerializeField] int jumpStep = 1;
        [SerializeField] float jumpHeight = 2f;
        [SerializeField] [Range(0, 1f)] float landThreshold = 0.15f;
        [SerializeField] [Range(0, 1f)] float perfectThreshold = 0.07f;


        public float LandThreshold => landThreshold;

        public float PerfectThreshold => perfectThreshold;

        public float DangerSpeedFactor => dangerSpeedFactor;

        public int JumpStep => jumpStep;

        public float BaseSpeed => baseSpeed;

        public float JumpHeight => jumpHeight;

        public int JumpsToMaxSpeed => jumpsToMaxSpeed;

        public float SpeedBoostFactor => speedBoostFactor;

        public int Lives => lives;
    }
}