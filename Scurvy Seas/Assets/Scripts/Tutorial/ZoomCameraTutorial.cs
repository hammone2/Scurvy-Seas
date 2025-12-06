using UnityEngine;

public class ZoomCameraTutorial : TutorialTypeBaseClass
{
    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            thisPopup.isInProgress = true;
        }
        else
        {
            thisPopup.isInProgress = false;
        }
    }
}
