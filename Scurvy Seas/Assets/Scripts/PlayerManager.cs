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
    [SerializeField] LayerMask clickableLayers;
    [SerializeField] GameObject wheelDisabledUI;
    [SerializeField] GameObject sailsDisabledUI;
    [SerializeField] Healthbar healthBar;
    [SerializeField] GameObject nextLevelButton;

    public ShipMovement playerShip;

    [SerializeField] Crewmate selectedCrewmate;
    private bool isCrewmateSelected = false;

    private Camera playerCamera;
    public Camera inventoryCamera;
    private float rayCastMaxDist = 777f;

    private bool isInventoryOpen = false;
    public InventorySystem inventorySystem;


    //item pickup stuff
    [SerializeField] private GameObject itemDropSearcherPrefab;
    private ItemDropSearcher itemDropSearcher;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerCamera = Camera.main;
        playerShip.SetPlayerManager(this);

        GameObject searcher = Instantiate(itemDropSearcherPrefab, playerShip.transform);
        itemDropSearcher = searcher.GetComponent<ItemDropSearcher>();
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
            OnMouseLeftClicked();
        }

        //move the crewmate
        if (Input.GetMouseButtonDown(1))
        {
            MoveToMouseClick();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveData data = SaveManager.LoadGame();
            if (data != null)
            {
                playerShip.Load(data);
            }
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

    public void SetSelectedCrewmate(Crewmate _crewmate)
    {
        if (isCrewmateSelected)
            selectedCrewmate.ToggleOutline();

        selectedCrewmate = _crewmate;
        isCrewmateSelected = true;
    }

    private void OnMouseLeftClicked()
    {
        Collider hit;

        if (!isInventoryOpen) 
        {
            hit = ShootRaycast(playerCamera);
        }
        else 
        {
            hit = ShootRaycast(inventoryCamera);
        }

        if (hit != null)
        {
            Clickable clickableObject = hit.GetComponent<Clickable>();
            if (clickableObject != null)
            {
                clickableObject.Clicked();
            }
        }
        else if (isCrewmateSelected)
        {
            isCrewmateSelected = false;
            selectedCrewmate.ToggleOutline();
        }
    }

    private Collider ShootRaycast(Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayCastMaxDist, clickableLayers))
        {
            return hit.collider;
        }
        return null;
    }

    public void SetIsInventoryOpen()
    {
        isInventoryOpen = !isInventoryOpen;
    }

    public void ActivateNextLevelButton()
    {
        nextLevelButton.SetActive(true);
    }
}
