using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cannon : MonoBehaviour
{
    public Task task;
    public GameObject cannonBallPrefab;

    private bool hasJustFired = false;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private Image reloadIndicator;
    [SerializeField] private float launchForce = 50f;
    [SerializeField] private Transform projectileSpawner;
    [SerializeField] private LayerMask layersToHit;

    public float elapsedTime = 0f;
    private Coroutine reloadCoroutine;

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
        if (!Physics.Raycast(projectileSpawner.position, projectileSpawner.forward, out hit, 100f, layersToHit))
            return;

        if (!hit.collider.CompareTag("Enemy"))
            return;

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
