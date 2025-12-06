using UnityEngine;

public class SelectCrewmateTutorial : TutorialTypeBaseClass
{
    private float iteration = 0f;
    private bool isSelected = false;

    protected override void Start()
    {
        base.Start();
        PlayerManager.instance.OnSelectedCrewmate += OnCrewmateSelected;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!isSelected)
                return;

            iteration += 1f;
            thisPopup.UpdateProgress(iteration);
        }
    }

    private void OnCrewmateSelected(bool b)
    {
        isSelected = b;
    }
}
