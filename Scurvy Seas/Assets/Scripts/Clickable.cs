using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{
    public UnityEvent OnClicked;

    public void Clicked()
    {
        OnClicked?.Invoke();
    }
}
