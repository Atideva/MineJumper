using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Misc
{
    public class FadeText : MonoBehaviour
    {
        [SerializeField] private bool hide;
        [SerializeField] private TextMeshProUGUI txt;
        [SerializeField] private float delay;
        [SerializeField] private float fadeTime;

        void Start()
        {
            if (hide) txt.DOColor(Color.clear, fadeTime).SetDelay(delay);
        }
    }
}