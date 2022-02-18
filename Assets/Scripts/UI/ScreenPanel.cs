using UnityEngine;
using UnityEngine.UI;

public class ScreenPanel : MonoBehaviour
{
    [SerializeField] private Button _armorButton;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _finishLevelPanel;

    public Button ArmorButton => _armorButton;

    private void Start()
    {
        _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        _menuPanel.SetActive(false);
        _finishLevelPanel.SetActive(false);
    }

    private void OnPauseButtonClicked()
    {
        _menuPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowFinishPanel()
    {
        Invoke(nameof(ShowFinish), 2);
        Invoke(nameof(HideFinishPanel), 5);
    }
    
    private void ShowFinish()
    {
        _finishLevelPanel.gameObject.SetActive(true);
    } 
    
    private void HideFinishPanel()
    {
        _finishLevelPanel.gameObject.SetActive(false);
    }
}
