using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))] //so the player can collide with the task (the ship will override the collision without this rb)
public class Task : MonoBehaviour
{
    private Collider triggerArea;

    public UnityEvent OnEnableTask;
    public UnityEvent OnDisableTask;

    private bool _isManned;
    private bool isManned
    {
        get { return _isManned; }
        set
        {
            if (_isManned == value) return; //prevent duplicate calls

            _isManned = value;

            if (_isManned) //use this if else to set color for task area indicator later
            {
                OnEnableTask.Invoke();
                Debug.Log("Task is manned");
            }
            else
            {
                OnDisableTask.Invoke();
                Debug.Log("Task is vacant");
            }
        }
    }

    private void Awake()
    {
        triggerArea = GetComponent<Collider>();

        //force isTrigger
        if (triggerArea)
            triggerArea.isTrigger = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            //force rigidbody attributes
            rb.isKinematic = true;
            rb.angularDamping = 0;
            rb.useGravity = false;
        }

        isManned = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Crewmate") && isManned == false)
        {
            isManned = true;
            other.GetComponent<Crewmate>().isDoingTask = isManned;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Crewmate") && isManned == true)
        {
            isManned = false;
            other.GetComponent<Crewmate>().isDoingTask = isManned;
        }
    }
}
