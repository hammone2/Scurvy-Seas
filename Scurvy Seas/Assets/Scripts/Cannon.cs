using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Cannon : MonoBehaviour
{
    public Task task;
    public GameObject cannonBallPrefab;
    public int damage = 1;

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
    [SerializeField] private Transform raycastShooter;
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private GameObject rangeIndicator;
    [SerializeField] private SpriteRenderer rangeIcon;
    [SerializeField] private ParticleSystem particles;

    public float elapsedTime = 0f;
    private Coroutine reloadCoroutine;

    /*/Line render stuff
    private LineRenderer lineRenderer;
    private int numPoints = 50;
    private float timeBetweenPoints = 0.1f;*/

    private void Start()
    {
        rangeIndicator.transform.localPosition = new Vector3(0f,0.5f,range);

        /*if (!task.isOwnedByPlayer)
            return;
        
        lineRenderer = GetComponent<LineRenderer>();
        CalculateTrajectoryLine();*/
    }

    private void Update()
    {
        if (task.GetIsManned())
        {
            if (!hasJustFired)
                FireCannon();
        }

        //CalculateTrajectoryLine();
    }

    private bool IsRaycastCollidingWithEnemy()
    {
        RaycastHit hit;
        if (!Physics.Raycast(raycastShooter.position, raycastShooter.forward, out hit, range, layersToHit))
        {
            inRange = false;
            return false;
        }

        string tag = task.isOwnedByPlayer ? "Enemy" : "Player";
        if (!hit.collider.CompareTag(tag))
        {
            inRange = false;
            return false;
        }

        inRange = true;
        return true;
    }

    private void FireCannon()
    {
        if (!IsRaycastCollidingWithEnemy())
            return;

        //do we have cannonballs?
        CannonballItem cannonballItem = null;
        if (task.isOwnedByPlayer)
        {
            InventorySystem inventory = PlayerManager.instance.inventorySystem;
            cannonballItem = inventory.FindFirstItemOfClass<CannonballItem>();
            if (cannonballItem == null)
                return;
        }
        

        hasJustFired = true;

        //temporary spawning code, use object pool later
        GameObject ball = Instantiate(cannonBallPrefab, projectileSpawner.position, projectileSpawner.rotation);
        if (ball.GetComponent<Rigidbody>())
            ball.GetComponent<Rigidbody>().AddForce(projectileSpawner.forward * launchForce, ForceMode.VelocityChange); //implement a range calculation later using the salvaged steel artillery code

        ball.GetComponent<Projectile>().SetDamage(damage);

        particles.Play();

        if (task.isOwnedByPlayer)
        {
            CameraController.instance.cameraShake.ScreenShake(0.5f);

            InventoryItem item = cannonballItem.GetComponent<InventoryItem>();
            int stackValue = item.stack - 1;
            item.SetStack(stackValue);

            reloadIndicator.fillAmount = 0f;

            //reload
            if (!HasCannonBall())
                return;
        }

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

    /*private void CalculateTrajectoryLine()
    {
        lineRenderer.positionCount = (int)numPoints;
        List<Vector3> points = new List<Vector3>();
        Vector3 startingPosition = projectileSpawner.position;
        Vector3 startingVelocity = projectileSpawner.forward * launchForce;
        for (float t = 0; t < numPoints; t += timeBetweenPoints)
        {
            Vector3 newPoint = startingPosition + t * startingVelocity;
            newPoint.y = startingPosition.y + startingVelocity.y * t + Physics.gravity.y / 2f * t * t;
            points.Add(newPoint);

            if (Physics.OverlapSphere(newPoint, 2, layersToHit).Length > 0)
            {
                lineRenderer.positionCount = points.Count;
                break;
            }
        }

        lineRenderer.SetPositions(points.ToArray());
    }*/
}
