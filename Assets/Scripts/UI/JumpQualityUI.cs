using Configs;
using DamageNumbersPro;
using Jump;
using PlayerScripts;
using UnityEngine;

namespace UI
{
    public class JumpQualityUI : MonoBehaviour
    {
        [Header("Setup")]
        public DamageNumber textPrefab;
        public RectTransform spawnPos;

        [Header("DEBUG")]
        public KeyCode testKey;
        private GameConfig _config;

        public void Init(GameConfig config, Player player)
        {
            _config = config;
            player.OnJump += OnJumpEnd;
        }

        void OnJumpEnd(JumpQualityType quality)
        {
            SpawnJumpMessage(quality);
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(testKey))
            {
                Spawn("Test Jump Message", Color.HSVToRGB(Random.value, 0.5f, 1f));
            }
        }
#endif

        void SpawnJumpMessage(JumpQualityType quality)
        {
            var message = _config.Messages.GetJumpData(quality);
            if (message == null) return;

            var r = Random.Range(0, message.content.Count);
            var text = message.content[r];
            var color = message.color;

            Spawn(text, color);
        }

        public void Spawn(string text, Color clr)
        {
            var newDamageNumber = textPrefab.Spawn(Vector3.zero);
            newDamageNumber.SetAnchoredPosition(spawnPos, Vector2.zero);
            newDamageNumber.enableFollowing = true;
            newDamageNumber.followedTarget = spawnPos;
            newDamageNumber.leftText = text;
            newDamageNumber.SetColor(clr);
        }
    }
}