using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts
{
    public class PanelChanger : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;

        public void ChangePanelWithAnim(GameObject fromGo, GameObject toGo)
        {
            var seq = DOTween.Sequence();

            seq.Append(fadeImage.DOFade(1f, 1f).OnStart(() => fadeImage.enabled = true)
                .OnComplete(() => { ChangePanel(fromGo, toGo); }));
            seq.Append(fadeImage.DOFade(0f, 1f).OnComplete(() => fadeImage.enabled = false));
        }

        internal void ChangePanel(GameObject fromGo, GameObject toGo)
        {
            fromGo.SetActive(false);
            toGo.SetActive(true);
        }
    }
}