using UnityEngine;

public class FinishLevelPanel : MonoBehaviour
{
    [SerializeField] private Animator _animation;
    
    private void OnEnable()
    {
        _animation.Play("Fading");
    }
}
