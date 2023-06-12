using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Scripts
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private PanelChanger panelChanger;
        [SerializeField] private GameObject firstPanel;
        [SerializeField] private GameObject secondPanel;
        [SerializeField] private GameObject thirdPanel;
        [SerializeField] private QuickSettings quickSettings;
        
        public void OnStartButtonTapped()
        {
            panelChanger.ChangePanel(firstPanel,secondPanel);
        }
        public void OnGetBackToMainMenuButtonTapped()
        {
            panelChanger.ChangePanel(secondPanel,firstPanel);
        }
        
        public void OnNewGameButtonTapped()
        {
            panelChanger.ChangePanelWithAnim(secondPanel,thirdPanel);
        }
        
        public void OnGetBackToSecondPanelButtonTapped()
        {
            panelChanger.ChangePanelWithAnim(thirdPanel,secondPanel);
        }

        public void OnStartGameButtonTapped()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }

        public void OnCreditsButtonDown()
        {
            Application.OpenURL("https://github.com/xtixi");
        }

        public void OnSettingsButtonDown()
        {
            quickSettings.OpenPanel();
        }
    }
}
