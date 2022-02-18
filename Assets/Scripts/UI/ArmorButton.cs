using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class
    ArmorButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    private readonly float _unpressedTime = 2;
    private Coroutine _pressed;

    public event UnityAction ArmorPressed;
    public event UnityAction ArmorUnpressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        ArmorPressed?.Invoke();
        _pressed = StartCoroutine(StartHoldTimer());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UnpressButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnpressButton();
    }


    private IEnumerator StartHoldTimer()
    {
        yield return new WaitForSeconds(_unpressedTime);
        UnpressButton();
    }

    private void UnpressButton()
    {
        ArmorUnpressed?.Invoke();
        if (_pressed != null)
            StopCoroutine(_pressed);
    }
}