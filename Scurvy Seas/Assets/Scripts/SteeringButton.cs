using UnityEngine;
using UnityEngine.EventSystems;

public class SteeringButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int direction = 0;

    public void OnPointerDown(PointerEventData eventData)
    {
        PlayerManager.instance.HandleSteer(direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PlayerManager.instance.HandleSteer(0);
    }
}
