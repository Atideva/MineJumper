using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerSounds : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private List<AudioClip> stoneSounds;
    
        public void PlayStone()
        {
            var r = Random.Range(0, stoneSounds.Count);
            source.PlayOneShot(stoneSounds[r]);
        }
    
    }
}