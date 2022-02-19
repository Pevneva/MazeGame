using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private Material _usual;
    [SerializeField] private Material _armor;
    [SerializeField] private GameObject _winFx;

    private ScreenPanel _screen;
    private MeshRenderer _meshRenderer;
    private bool _isImmortal;

    public event UnityAction Died;
    public event UnityAction FinishReached;

    private void Start()
    {
        _screen = FindObjectOfType<ScreenPanel>();
        _isImmortal = false;
        _meshRenderer = GetComponent<MeshRenderer>();
        _screen.ArmorButton.GetComponent<ArmorButton>().ArmorPressed += OnArmorPressed;
        _screen.ArmorButton.GetComponent<ArmorButton>().ArmorUnpressed += OnArmorUnpressed;
    }

    private void OnArmorUnpressed()
    {
        _isImmortal = false;
        _meshRenderer.sharedMaterial = _usual;
    }

    private void OnArmorPressed()
    {
        _isImmortal = true;
        _meshRenderer.sharedMaterial = _armor;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<DeathArea>(out DeathArea deathArea))
        {
            if (_isImmortal == false)
            {
                Die();
            }
        }

        if (other.gameObject.TryGetComponent<FinishArea>(out FinishArea finishArea))
        {
            Win();
        }
    }

    private void Win()
    {
        FinishReached?.Invoke();
        Invoke(nameof(ShowWinFx), 0.35f);
        _screen.ArmorButton.GetComponent<ArmorButton>().ArmorPressed -= OnArmorPressed;
        _screen.ArmorButton.GetComponent<ArmorButton>().ArmorUnpressed -= OnArmorUnpressed;
    }

    private void Die()
    {
        Died?.Invoke();
        _screen.ArmorButton.GetComponent<ArmorButton>().ArmorPressed -= OnArmorPressed;
        _screen.ArmorButton.GetComponent<ArmorButton>().ArmorUnpressed -= OnArmorUnpressed;
        Destroy(gameObject, ParamsController.DelayDeath);
    }

    private void ShowWinFx()
    {
        _winFx.SetActive(true);
        Invoke(nameof(HideWinfx), 3);
    }

    private void HideWinfx()
    {
        _winFx.SetActive(false);
    }
}