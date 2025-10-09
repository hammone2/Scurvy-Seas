using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))] //so the player can collide with the task (the ship will override the collision without this rb)
public class Task : MonoBehaviour
{
    private Collider triggerArea;

    public UnityEvent OnEnableTask;
    public UnityEvent OnDisableTask;
    private Crewmate crewmateWorkingThisTask;
    private List<Crewmate> crewmatesInArea = new List<Crewmate>(); 

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Crewmate"))
        {
            Crewmate crewmate = other.GetComponent<Crewmate>();
            
            if (crewmateWorkingThisTask == null) //is nobody working this task?
                SetManned(true, crewmate);

            crewmatesInArea.Add(crewmate);
            Debug.Log(crewmatesInArea.Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Crewmate"))
        {
            Crewmate crewmate = other.GetComponent<Crewmate>();
            
            if (crewmateWorkingThisTask == crewmate) //did our crewmate working the task leave?
                SetManned(false, crewmate);

            crewmatesInArea.Remove(crewmate);
            Debug.Log(crewmatesInArea.Count);

            if (crewmatesInArea.Count == 0)
                return;
                
            //have next crewmate in the area man the task station
            Crewmate newCrewmate = crewmatesInArea[0];
            SetManned(true, newCrewmate);
        }
    }

    private void SetManned(bool _isManned, Crewmate _crewmate)
    {
        isManned = _isManned;
        _crewmate.isDoingTask = _isManned;

        if (!_isManned)
        {
            crewmateWorkingThisTask = null;
            return;
        }
        crewmateWorkingThisTask = _crewmate;
    }

    public bool GetIsManned()
    {
        return isManned;
    }
}
