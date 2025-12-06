using UnityEngine;

public class TutorialTypeBaseClass : MonoBehaviour
{
    protected TutorialPopup thisPopup;

    protected virtual void Start()
    {
        thisPopup = GetComponent<TutorialPopup>();
    }
}
