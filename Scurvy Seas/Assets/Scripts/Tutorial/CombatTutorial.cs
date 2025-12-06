using UnityEngine;

public class CombatTutorial : TutorialTypeBaseClass
{
    protected override void Start()
    {
        base.Start();
        LevelManager.instance.OnEncounterComplete.AddListener(OnAllEnemiesKilled);
    }

    private void OnAllEnemiesKilled()
    {
        this.thisPopup.OnComplete();
    }
}
