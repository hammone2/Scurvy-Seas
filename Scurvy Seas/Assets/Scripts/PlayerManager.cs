using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public int steeringDirection;
    public TextMeshProUGUI degreesText;
    [SerializeField] float rotationSpeed;
    [SerializeField] GameObject wheelSprite;
    [SerializeField] Slider sailSlider;
    [SerializeField] LayerMask floorLayers;
    [SerializeField] LayerMask crewmateLayer;
    [SerializeField] GameObject wheelDisabledUI;
    [SerializeField] GameObject sailsDisabledUI;
    [SerializeField] Healthbar healthBar;

    public ShipMovement playerShip;

    public Crewmate testCrewmate; //delete this later

    [SerializeField] Crewmate selectedCrewmate;
    private bool isCrewmateSelected = false;

    private Camera playerCamera;
    private float rayCastMaxDist = 777f;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerCamera = Camera.main;
        playerShip.SetPlayerManager(this);
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
            SelectCrewMate();
        }

        //move the crewmate
        if (Input.GetMouseButtonDown(1))
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

    public void SetSteerTask(bool _canSteer)
    {
        wheelDisabledUI.SetActive(_canSteer);
    }

    public void SetSailTask(bool _canSetSailLength)
    {
        sailsDisabledUI.SetActive(_canSetSailLength);
    }

    void MoveToMouseClick()
    {
        if (!isCrewmateSelected)
            return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayCastMaxDist, floorLayers))
        {
            selectedCrewmate.SetNavDestination(hit.point);
        }
    }

    void SelectCrewMate()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayCastMaxDist, crewmateLayer))
        {
            Crewmate crewmate = hit.collider.GetComponent<Crewmate>();
            if (crewmate != null)
            {
                selectedCrewmate = crewmate;
                isCrewmateSelected = true;
            }
        }
    }
}
