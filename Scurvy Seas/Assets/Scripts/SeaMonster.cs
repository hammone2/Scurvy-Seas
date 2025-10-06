using UnityEngine;

public class SeaMonster : MonoBehaviour, IKillable
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawner;

    [SerializeField] private float rotationSpeed;

    private bool hasJustFired = false;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float launchForce = 50f;

    [SerializeField] private GameObject itemDrop;

    private Transform player;

    private void Start()
    {
        player = PlayerManager.instance.playerShip.transform;
    }


    private void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }



        if (!hasJustFired)
            FireCannon();
    }

    private void FireCannon()
    {
        hasJustFired = true;

        //temporary spawning code, use object pool later
        GameObject ball = Instantiate(projectile, projectileSpawner.position, projectileSpawner.rotation);
        if (ball.GetComponent<Rigidbody>())
            ball.GetComponent<Rigidbody>().AddForce(projectileSpawner.forward * launchForce, ForceMode.VelocityChange); //implement a range calculation later using the salvaged steel artillery code

        Invoke("CoolDown", fireRate);
    }

    private void CoolDown()
    {
        hasJustFired = false;
    }

    public void Die()
    {
        Instantiate(itemDrop);
        itemDrop.transform.position = transform.position;
        Destroy(gameObject);
    }
}
