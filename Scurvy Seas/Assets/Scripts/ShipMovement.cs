using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipMovement : MonoBehaviour, IKillable //add a ship base class later that controls non-movement stuff
{
    public int steeringDirection = 0;

    public float moveSpeed = 100f;
    public float thrustAmount = 0f;
    public float turnStrength = 0.1f;

    public bool canSteer = false;
    public bool canSetSailLength = false;
    public bool isOwnedByPlayer = false;

    private Rigidbody rb;
    private List<NavMeshAgent> agentsOnShip = new List<NavMeshAgent>();
    private List<Task> taskStations = new List<Task>();

    private PlayerManager playerManager; //might change this to a conroller base class that AIShip and PlayerManager inherit from

    [SerializeField] private Transform itemDisposal;
    [SerializeField] private Transform crewmateContainer;
    [SerializeField] private Transform taskStationContainer;
    [SerializeField] private GameObject crewmatePrefab;

    [SerializeField] private Damagable damagableComponent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        //initial get crewmates
        for (int i = 0; i < crewmateContainer.childCount; i++)
        {
            GameObject obj = crewmateContainer.GetChild(i).gameObject;
            if (obj.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                agentsOnShip.Add(agent);
                Crewmate crewmate = obj.GetComponent<Crewmate>();
                crewmate.ReparentNavPoint(transform);
                crewmate.isOwnedByPlayer = isOwnedByPlayer;
            }
        }

        //get task stations
        for (int i = 0; i < taskStationContainer.childCount; i++)
        {
            GameObject obj = taskStationContainer.GetChild(i).gameObject;
            Task taskStation = obj.GetComponentInChildren<Task>();
            if (taskStation != null)
            {
                taskStations.Add(taskStation);
                taskStation.isOwnedByPlayer = isOwnedByPlayer;
                taskStation.ConnectVisibilityEvent(isOwnedByPlayer);
            }
        }

        //shake the camera when damage is taken if this is the player
        if (isOwnedByPlayer)
        {
            damagableComponent.OnDamageTaken += CameraController.instance.cameraShake.ScreenShake;
        }
    }

    void FixedUpdate()
    {
        MoveShip();
        SteerShip();

        //Adjust agent velocity to account for ship velocity
        for (int i = 0; i < agentsOnShip.Count; i++)
        {
            if (agentsOnShip[i] == null) continue;

            Vector3 adjustedVelocity = agentsOnShip[i].desiredVelocity + rb.linearVelocity;
            agentsOnShip[i].velocity = adjustedVelocity;
        }
    }

    public void HandleThrust(float amount)
    {
        if (!canSetSailLength)
            return;
        thrustAmount = amount;
    }

    public void HandleSteer(int direction)
    {
        steeringDirection = -direction;
    }

    public void SetSteerTask(bool _canSteer)
    {
        canSteer = _canSteer;

        if (!isOwnedByPlayer)
            return;

        if (playerManager != null)
            playerManager.SetSteerTask(!_canSteer);
    }

    public void SetSailTask(bool _canSetSailLength)
    {
        canSetSailLength = _canSetSailLength;

        if (!isOwnedByPlayer)
            return;

        if (playerManager != null)
            playerManager.SetSailTask(!_canSetSailLength);
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
        if (!canSteer)
            return;

        if (rb.linearVelocity.magnitude > 0.1f) //avoid rotation when stationary
        {
            float currentSpeed = rb.linearVelocity.magnitude;
            float turnSpeed = turnStrength * currentSpeed;

            float rotation = steeringDirection * turnSpeed * Time.deltaTime;
            rb.AddTorque(Vector3.up * rotation, ForceMode.VelocityChange);
        }
    }

    public void SetPlayerManager(PlayerManager _playerManager)
    {
        playerManager = _playerManager;
        isOwnedByPlayer = true;
    }

    public void Heal(int amount)
    {
        damagableComponent.Heal(amount);
    }

    public void Die()
    {
        if (isOwnedByPlayer)
            GameManager.instance.EndGame();
    }

    public void ThrowItemOverboard(GameObject item)
    {
        item.transform.position = itemDisposal.position;
    }

    public List<NavMeshAgent> GetCrewmates()
    {
        return agentsOnShip;
    }

    public List<Task> GetTaskStations()
    {
        return taskStations;
    }

    public ShipData Save()
    {
        ShipData shipData = new ShipData();
        shipData.Crewmates = new CrewmateData[agentsOnShip.Count];

        for (int i = 0; i < agentsOnShip.Count; i++)
        {
            CrewmateData newCrewmate = new CrewmateData();
            Crewmate crewmate = agentsOnShip[i].GetComponent<Crewmate>();
            newCrewmate.Name = crewmate.gameObject.name;

            newCrewmate.Position = new float[]
            {
                crewmate.transform.localPosition.x,
                crewmate.transform.localPosition.y,
                crewmate.transform.localPosition.z
            };

            shipData.Crewmates[i] = newCrewmate;
        }

        shipData.ShipHealth = damagableComponent.health;

        return shipData;
    }
    public void Load(SaveData saveData)
    {
        //clear old agents
        foreach (var agent in agentsOnShip)
        {
            Destroy(agent.GetComponent<Crewmate>().GetNavPoint());
            Destroy(agent.gameObject);
        }
        agentsOnShip.Clear();


        //spawn new agents from file
        ShipData shipData = saveData.PlayerShip;

        for (int i = 0; i < shipData.Crewmates.Length; i++)
        {
            CrewmateData crewmateData = shipData.Crewmates[i];
            GameObject newCrewmate = Instantiate(crewmatePrefab, crewmateContainer);
            newCrewmate.name = crewmateData.Name;
            newCrewmate.transform.localPosition = new Vector3(crewmateData.Position[0], crewmateData.Position[1], crewmateData.Position[2]);

            newCrewmate.GetComponent<Crewmate>().GetNavPoint().transform.position = newCrewmate.transform.position;

            if (newCrewmate.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                agentsOnShip.Add(agent);
                Crewmate crewmate = newCrewmate.GetComponent<Crewmate>();
                crewmate.ReparentNavPoint(transform);
                crewmate.isOwnedByPlayer = isOwnedByPlayer;
            }
        }

        damagableComponent.health = shipData.ShipHealth;
    }
}