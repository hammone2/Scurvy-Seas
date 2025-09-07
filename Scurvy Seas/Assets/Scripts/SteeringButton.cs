using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SteeringButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int direction = 0;
    private bool isHeld = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        SteeringManager.instance.HandleSteer(direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SteeringManager.instance.HandleSteer(0);
    }
}
