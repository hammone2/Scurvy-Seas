using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int steeringDirection;
    public TextMeshProUGUI degreesText;
    [SerializeField] float rotationSpeed;
    [SerializeField] GameObject wheelSprite;
    [SerializeField] Slider sailSlider;

    public ShipMovement playerShip;

    public Crewmate testCrewmate; //delete this later

    private Camera playerCamera;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        if (steeringDirection != 0)
        {
            float currentAngle = Mathf.DeltaAngle(0, wheelSprite.transform.eulerAngles.z);

            wheelSprite.transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime * steeringDirection);
        }

        if (Input.GetMouseButtonDown(0))
        {
            MoveToMouseClick();
        }
    }

    public void HandleSteer(int direction)
    {
        steeringDirection = -direction;
        playerShip.HandleSteer(-direction);
    }

    public void HandleThrust()
    {
        float amount = sailSlider.value;
        playerShip.HandleThrust(amount);
    }

    void MoveToMouseClick()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            testCrewmate.GetComponent<NavMeshAgent>().SetDestination(hit.point); //make it a local position on tne ship
        }
    }
}
