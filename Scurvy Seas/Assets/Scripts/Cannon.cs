using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cannon : MonoBehaviour
{
    public Task task;
    public GameObject cannonBallPrefab;

    private bool hasJustFired = false;


    private bool _inRange = false;
    private bool inRange
    {
        get { return _inRange; }
        set {
            if (_inRange == value) return;
            _inRange = value;
            
            if (!_inRange)
                rangeIcon.color = Color.white;
            else
                rangeIcon.color = Color.yellow;
        }
    }

    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float range = 100f;
    [SerializeField] private Image reloadIndicator;
    [SerializeField] private float launchForce = 50f;
    [SerializeField] private Transform projectileSpawner;
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private GameObject rangeIndicator;
    [SerializeField] private SpriteRenderer rangeIcon;

    public float elapsedTime = 0f;
    private Coroutine reloadCoroutine;

    private void Start()
    {
        rangeIndicator.transform.localPosition = new Vector3(0f,0.5f,range);
    }

    private void Update()
    {
        if (task.GetIsManned())
        {
            if (!hasJustFired)
                FireCannon();
        }
    }

    private void FireCannon()
    {
        RaycastHit hit;
        if (!Physics.Raycast(projectileSpawner.position, projectileSpawner.forward, out hit, range, layersToHit))
        {
            inRange = false;
            return;
        }

        if (!hit.collider.CompareTag("Enemy"))
        {
            inRange = false; 
            return;
        }

        inRange = true;

        //do we have cannonballs?
        InventorySystem inventory = PlayerManager.instance.inventorySystem;
        CannonballItem cannonballItem = inventory.FindFirstItemOfClass<CannonballItem>();
        if (cannonballItem == null)
            return;

        hasJustFired = true;

        //temporary spawning code, use object pool later
        GameObject ball = Instantiate(cannonBallPrefab, projectileSpawner.position, projectileSpawner.rotation);
        if (ball.GetComponent<Rigidbody>())
            ball.GetComponent<Rigidbody>().AddForce(projectileSpawner.forward * launchForce, ForceMode.VelocityChange); //implement a range calculation later using the salvaged steel artillery code


        InventoryItem item = cannonballItem.GetComponent<InventoryItem>();
        int stackValue = item.stack - 1;
        item.SetStack(stackValue);

        reloadIndicator.fillAmount = 0f;

        //reload
        if (!HasCannonBall())
            return;

        elapsedTime = 0;
        reloadCoroutine = StartCoroutine(Reload());
    }

    private CannonballItem HasCannonBall()
    {
        InventorySystem inventory = PlayerManager.instance.inventorySystem;
        CannonballItem cannonballItem = inventory.FindFirstItemOfClass<CannonballItem>();
        return cannonballItem;
    }

    private IEnumerator Reload()
    {
        while (elapsedTime < fireRate)
        {
            reloadIndicator.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / fireRate);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        reloadIndicator.fillAmount = 1;
        hasJustFired = false;
    }

    public void PauseReload()
    {
        if (reloadCoroutine == null)
            return;

        StopCoroutine(reloadCoroutine);
    }

    public void ReadyTask()
    {
        if (!HasCannonBall())
            return;

        reloadCoroutine = StartCoroutine(Reload());
    }
}
