using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SpeedBoostUI : MonoBehaviour
    {
        [SerializeField] private Slider normalSlider;
        [SerializeField] private Slider dangerSlider;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color dangerColor;
        [SerializeField] Image normalSliderImage;
        [Header("DEBUG")]
        public float lastValue;
        public List<float> allValues = new();

        public void Init(float dangerValue, Player player)
        {
            dangerSlider.value = dangerValue;
            normalSliderImage.color = normalColor;
            Refresh(0);
            
            player.OnSpeedBoostChanged += OnBoostChange;
        }

        void OnBoostChange(float boost)
        {
            Refresh(boost);
        }

        public void Refresh(float value)
        {
            normalSlider.value = value;
            normalSliderImage.color = value < dangerSlider.value ? normalColor : dangerColor;

            SetLast(value);
        }

        void SetLast(float value)
        {
            lastValue = value;
#if UNITY_EDITOR
            allValues.Add(value);
#endif
        }
    }
}