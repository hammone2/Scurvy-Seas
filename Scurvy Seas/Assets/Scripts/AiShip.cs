using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AiShip : MonoBehaviour
{
    private ShipMovement ship;

    [SerializeField] private float range = 50f;
    private Transform target;
    private Rigidbody rb;

    private enum ShipState
    {
        Pursuing,
        Engaging
    }
    private ShipState shipState;

    private void Awake()
    {
        shipState = ShipState.Pursuing;
        ship = GetComponent<ShipMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        target = PlayerManager.instance.playerShip.transform; 
        ship.canSetSailLength = true; //this is temporary make sure the crewmate is manning the sail task
        Invoke("SendCrewToBattleStations", 0.25f);
    }

    private void Update()
    {
        if (!target)
            return;
        
        Vector3 delta = (target.position - transform.position).normalized;
        Vector3 cross = Vector3.Cross(delta, transform.forward);

        switch (shipState)
        {
            case ShipState.Pursuing:
                if (Vector3.Distance(transform.position, target.position) <= range)
                {
                    shipState = ShipState.Engaging;
                    Debug.LogWarning("State = " + shipState.ToString());
                }

                if (cross == Vector3.zero)
                {
                    // Target is straight ahead
                    ship.HandleSteer(0);
                }
                else if (cross.y > 0)
                {
                    // Target is to the right
                    ship.HandleSteer(1);
                }
                else
                {
                    // Target is to the left
                    ship.HandleSteer(-1);
                }


                break;

            case ShipState.Engaging:
                if (Vector3.Distance(transform.position, target.position) > range)
                {
                    shipState = ShipState.Pursuing;
                    Debug.LogWarning("State = " + shipState.ToString());
                }

                if (cross == Vector3.zero)
                {
                    // Target is straight ahead
                    ship.HandleSteer(0);
                }
                else if (cross.y > 0)
                {
                    // Target is to the right
                    HandleSteer(Vector3.right);
                }
                else
                {
                    // Target is to the left
                    HandleSteer(Vector3.left);
                }

                break;
        }
    }

    private void HandleSteer(Vector3 sideDirection)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        rotation = Quaternion.Euler(0, angle + sideDirection.x * 180f, 0) * rotation;

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotation, Time.deltaTime));
    }

    private void SendCrewToBattleStations()
    {
        for (int i = 0; i < ship.GetTaskStations().Count; i++)
        {
            List<NavMeshAgent> crewmateList = ship.GetCrewmates();
            Crewmate crewmate = crewmateList[i].GetComponent<Crewmate>();

            if (crewmate != null)
            {
                crewmate.SetNavDestination(ship.GetTaskStations()[i].transform.position);
            }
        }
        ship.HandleThrust(1f);
    }
}
