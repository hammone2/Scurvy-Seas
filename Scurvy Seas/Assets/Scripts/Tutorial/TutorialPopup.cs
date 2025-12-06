using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private bool hasProgress = false;
    [SerializeField] private float maxProgress = 7f;
    private float elapsedTime = 0f;
    public bool isInProgress = false; //set this to true in the inspector to have a timed popup
    private bool isActivated = false;

    private void FixedUpdate()
    {
        if (!isActivated)
            return;

        if (isInProgress)
        {
            elapsedTime += Time.deltaTime;
            UpdateProgress(elapsedTime);
        }
    }

    public void Activate()
    {
        isActivated = true;
    }

    public void UpdateProgress(float newValue)
    {
        if (!hasProgress)
            return;

        if (!isActivated)
            return;

        progressSlider.value = newValue / maxProgress;

        if (progressSlider.value == 1)
            OnComplete();
    }

    public void OnComplete()
    {
        if (!isActivated)
            return;

        TutorialManager.Instance.NextPopup();
        Destroy(gameObject);
    }
}
