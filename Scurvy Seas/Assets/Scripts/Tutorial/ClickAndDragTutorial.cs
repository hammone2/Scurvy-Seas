using UnityEngine;

public class ClickAndDragTutorial : TutorialTypeBaseClass
{
    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            thisPopup.isInProgress = true;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            thisPopup.isInProgress = false;
        }
    }
}
