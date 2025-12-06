using UnityEngine;

public class WheelControlTutorial : TutorialTypeBaseClass
{
    private void Update()
    {
        float input = Input.GetAxis("Horizontal");
        if (input != 0)
            thisPopup.isInProgress = true;
        else
            thisPopup.isInProgress = false;
    }
}
