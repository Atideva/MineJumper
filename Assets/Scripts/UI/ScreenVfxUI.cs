using DG.Tweening;
using PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScreenVfxUI : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float vignetteAppearTime;
        [SerializeField] private float vignetteRedDelay;
        [SerializeField] private float vignetteBlackDelay;
        [SerializeField] private float blindDelay;
        [SerializeField] private float blindAppearTime;
       
        [Header("Setup")]
        [SerializeField] private Image vignetteRed;
        [SerializeField] private Image vignetteBlack;
        [SerializeField] private Image blindTop;
        [SerializeField] private Image blindBottom;


        public void Init(Player player)
        {
            DisableAll();

            player.OnLiveLost += PlayerLostLive;
            player.OnRespawn += PlayerRespawn;
        }

        void DisableAll()
        {
            vignetteRed.enabled = false;
            vignetteBlack.enabled = false;
            blindTop.enabled = false;
            blindBottom.enabled = false;
        }

        void EnableAll()
        {
            vignetteRed.enabled = true;
            vignetteBlack.enabled = true;
            blindTop.enabled = true;
            blindBottom.enabled = true;
        }


        void HideAll()
        {
            vignetteRed.color = Color.clear;
            vignetteBlack.color = Color.clear;
            blindTop.fillAmount = 0;
            blindBottom.fillAmount = 0;
        }

        void PlayerLostLive()
        {
            EnableAll();
            HideAll();

            vignetteRed.DOColor(Color.white, vignetteAppearTime).SetDelay(vignetteRedDelay);
            vignetteBlack.DOColor(Color.white, vignetteAppearTime).SetDelay(vignetteBlackDelay);
           
            blindTop.DOFillAmount(1, blindAppearTime).SetDelay(blindDelay);
            blindBottom.DOFillAmount(1, blindAppearTime).SetDelay(blindDelay);
        }

        void PlayerRespawn()
        {
            vignetteRed.color = Color.clear;
           
            blindTop.DOFillAmount(0, blindAppearTime);
            blindBottom.DOFillAmount(0, blindAppearTime);
         
            vignetteBlack.DOColor(Color.clear, vignetteAppearTime).SetDelay(blindAppearTime);
        }
    }
}