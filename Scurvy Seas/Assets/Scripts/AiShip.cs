using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AiShip : MonoBehaviour
{
    private ShipMovement ship;

    [SerializeField] private float range = 50f;
    private Transform target;

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
        
        Vector3 localTargetPos = transform.InverseTransformPoint(target.position); //translate targets global pos to local coordinates


        switch (shipState)
        {
            case ShipState.Pursuing:
                if (Vector3.Distance(transform.position, target.position) <= range)
                {
                    shipState = ShipState.Engaging;
                }


                //Steer the ship in the direction of the player

                    //check sides
                if (localTargetPos.x < 0)
                {
                    //target is on the right
                    ship.HandleSteer(1);
                    Debug.Log("Pursuing Right");
                }
                else
                {
                    //target is on the left
                    ship.HandleSteer(-1);
                    Debug.Log("Pursuing Left");
                }


                break;

            case ShipState.Engaging:
                if (Vector3.Distance(transform.position, target.position) > range)
                {
                    shipState = ShipState.Pursuing;
                }


                //Steer ship to have its guns face the player depending on the quadrant relative to this ship's position they are in

                    //check quadrants
                if (localTargetPos.z > 0 && localTargetPos.x < 0)
                {
                    // Target is to the front left
                    ship.HandleSteer(-1);
                    Debug.Log("Engaging Front Left");
                }
                else if (localTargetPos.z > 0 && localTargetPos.x > 0)
                {
                    // Target is to the front right
                    ship.HandleSteer(1);
                    Debug.Log("Engaging Front Right");
                }
                else if (localTargetPos.z < 0 && localTargetPos.x < 0)
                {
                    // Target is to the bottom left
                    ship.HandleSteer(-1);
                    Debug.Log("Engaging Bottom Left");
                }
                else if (localTargetPos.z < 0 && localTargetPos.x > 0)
                {
                    // Target is to the bottom right
                    ship.HandleSteer(1);
                    Debug.Log("Engaging Bottom Right");
                }


                break;
        }

        Debug.LogWarning("State = " + shipState.ToString());
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
