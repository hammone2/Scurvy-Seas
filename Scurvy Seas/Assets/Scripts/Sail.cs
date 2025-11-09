using UnityEngine;

public class Sail : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Update()
    {
        //This coide is for testing purposes, remove this later
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("RollUp");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Wind");
        }
    }
}
