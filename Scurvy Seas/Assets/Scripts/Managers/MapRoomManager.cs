using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MapRoomManager : MonoBehaviour
{
    [SerializeField] private Image fade;
    [SerializeField] private Animator map;
    [SerializeField] private Transform youAreHereIcon;
    [SerializeField] private Transform[] mapPositions;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color fadeColor = fade.color;
        float alpha = fadeColor.a;

        while (alpha > 0)
        {
            alpha = Mathf.MoveTowards(alpha, 0, Time.deltaTime);
            fadeColor.a = alpha;
            fade.color = fadeColor;
            yield return null;
        }
        map.SetTrigger("UnRoll");
        Invoke("UpdateMap", 8.25f);
    }

    private IEnumerator FadeIn()
    {
        fade.gameObject.SetActive(true);
        Color fadeColor = fade.color;
        float alpha = fadeColor.a;
        int target = 1;

        while (alpha != target)
        {
            alpha = Mathf.MoveTowards(alpha, target, Time.deltaTime);
            fadeColor.a = alpha;
            fade.color = fadeColor;
            yield return null;
        }
        GoToNextLevel();
    }

    private void UpdateMap()
    {
        int pos = GameManager.instance.GetCurrentLevel();
        //pos -= 1;
        Transform destination = mapPositions[pos];
        GameObject space = destination.gameObject;

        //is our destination a port?
        if (destination.gameObject.CompareTag("PortSpace"))
        {
            GameManager.instance.isNextLevelAPort = true;
        }
        else
        {
            GameManager.instance.isNextLevelAPort = false;
        }

        Vector3 startPos = mapPositions[pos - 1].position;
        Vector3 endPos = destination.position;
        StartCoroutine(FadeInIcon(startPos, endPos));
    }

    private IEnumerator FadeInIcon(Vector3 startPos, Vector3 endPos)
    {
        Color fadeColor = youAreHereIcon.gameObject.GetComponent<Renderer>().material.color;
        fadeColor.a = 0;
        youAreHereIcon.gameObject.GetComponent<Renderer>().material.color = fadeColor;
        float alpha = fadeColor.a;

        youAreHereIcon.position = startPos;

        while (alpha < 1)
        {
            alpha = Mathf.MoveTowards(alpha, 1, Time.deltaTime * 0.5f);
            fadeColor.a = alpha;
            youAreHereIcon.gameObject.GetComponent<Renderer>().material.color = fadeColor;
            yield return null;
        }

        StartCoroutine(MoveIcon(endPos));
    }

    private IEnumerator MoveIcon(Vector3 endPos)
    {
        Vector3 direction = youAreHereIcon.position - endPos;
        direction.y = 0;
        while (youAreHereIcon.position != endPos)
        {
            //rotate
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 180f * Time.deltaTime);

            //move
            youAreHereIcon.position = Vector3.MoveTowards(youAreHereIcon.position, endPos, Time.deltaTime * 0.25f);
            yield return null;
        }
        Invoke("Next", 3f);
    }

    private void Next()
    {
        StartCoroutine(FadeIn());
    }

    public void GoToNextLevel()
    {
        GameManager.instance.GoToNextLevel();
    }
}
