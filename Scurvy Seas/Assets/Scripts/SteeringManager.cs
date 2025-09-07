using TMPro;
using UnityEngine;

public class SteeringManager : MonoBehaviour
{
    public static SteeringManager instance;
    public int steeringDirection;

    private void Awake()
    {
        instance = this;
    }

    public void HandleSteer(int direction)
    {
        steeringDirection = direction;
    }
}
