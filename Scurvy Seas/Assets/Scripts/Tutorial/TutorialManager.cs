using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TutorialPopup[] popups;
    public Event OnPopupFinished;
    public static TutorialManager Instance;
    private int currentPopup = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        NextPopup();
    }

    public void NextPopup()
    {
        TutorialPopup popup = popups[currentPopup];

        if (!popup)
        {
            Destroy(gameObject); //tutorial complete
            return; //returning since Destroy() is executed when the function is finished
        }

        popup.gameObject.SetActive(true);
        popup.Activate();

        currentPopup++; //queue next popup for activation
    }
}
