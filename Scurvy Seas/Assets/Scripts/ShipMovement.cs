using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ShipMovement : MonoBehaviour
{
    public int steeringDirection = 0;

    public float moveSpeed = 100f;
    public float thrustAmount = 0f;
    public float turnStrength = 0.1f;

    private Rigidbody rb;
    private List<NavMeshAgent> agentsOnShip = new List<NavMeshAgent>();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();


        //initial get crewmates
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject obj = transform.GetChild(i).gameObject;
            if (obj.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                agentsOnShip.Add(agent);
                Crewmate crewmate = obj.GetComponent<Crewmate>();
                crewmate.ReparentNavPoint(transform);
            }
        }
    }

    void FixedUpdate()
    {
        MoveShip();
        SteerShip();

        //Adjust agent velocity to account for ship velocity
        for (int i = 0; i < agentsOnShip.Count; i++)
        {
            Vector3 adjustedVelocity = agentsOnShip[i].desiredVelocity + rb.linearVelocity;
            agentsOnShip[i].velocity = adjustedVelocity;
        }
    }

    public void HandleThrust(float amount)
    {
        thrustAmount = amount;
    }

    public void HandleSteer(int direction)
    {
        steeringDirection = -direction;
    }

    private void MoveShip()
    {
        if (thrustAmount <= 0)
            return;

        Vector3 forwardMovement = transform.forward * thrustAmount * moveSpeed * Time.deltaTime;

        rb.AddForce(forwardMovement, ForceMode.Acceleration);
    }

    private void SteerShip()
    {
        if (rb.linearVelocity.magnitude > 0.1f) //avoid rotation when stationary
        {
            float currentSpeed = rb.linearVelocity.magnitude;
            float turnSpeed = turnStrength * currentSpeed;

            float rotation = steeringDirection * turnSpeed * Time.deltaTime;
            rb.AddTorque(Vector3.up * rotation, ForceMode.VelocityChange);
        }
    }
}
