using UnityEngine;

public class SailControlTutorial : TutorialTypeBaseClass
{
    private void Update()
    {
        float input = Input.GetAxis("Vertical");
        if (input != 0)
            thisPopup.isInProgress = true;
        else
            thisPopup.isInProgress = false;
    }
}
