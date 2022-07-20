 
using UnityEngine;

[CreateAssetMenu(fileName = "Jump config", menuName = "Game/Jump Config")]
public class JumpConfig : ScriptableObject
{
   [Header("Config")]
   [SerializeField][Range(0,1f)] private float landThreshold;
   [SerializeField][Range(0,1f)] private float perfectThreshold;



   public float LandThreshold => landThreshold;

   public float PerfectThreshold => perfectThreshold;
}

/*

                -landThreshold                 LandAtCube                     +landThreshold
                    |------------------------------|------------------------------|
                    
                    
_________________| |______________________| |____________| |_____________________| |_________________________
       BeforeTime            Good              Perfect             Good             TooLate   
                           
*/