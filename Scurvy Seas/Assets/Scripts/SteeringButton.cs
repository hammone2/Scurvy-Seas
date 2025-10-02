using UnityEngine;

public class SteeringButton : MonoBehaviour
{
    public int direction = 0;
    private bool isSteering = false;

    public void ToggleSteer()
    {      
        int _direction;

        if (isSteering)
        {
            isSteering = false;
            _direction = 0;
        }
        else 
        { 
            isSteering = true;
            _direction = direction;
        }

        PlayerManager.instance.HandleSteer(_direction);
    }

    //for when we click on the other button
    public void DisableSteer()
    {
        isSteering = false;
    }
}
