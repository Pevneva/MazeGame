using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MenuPanel : MonoBehaviour
{
    [SerializeField] private Button _continue;
    [SerializeField] private Button _exit;
    [SerializeField] private Animator _animation;

    private Image _image;
    
    private void OnEnable()
    {
        _continue.onClick.AddListener(OnContinueButtonClicked);
        _exit.onClick.AddListener(OnExitButtonClicked);
        _animation.updateMode = AnimatorUpdateMode.UnscaledTime;
        _animation.Play("Fading");
    }
    
    private void OnDisable()
    {
        _continue.onClick.AddListener(OnContinueButtonClicked);
        _exit.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnContinueButtonClicked()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
