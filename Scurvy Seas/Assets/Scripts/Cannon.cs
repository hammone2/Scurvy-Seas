using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Task task;
    public GameObject cannonBallPrefab;

    private bool hasJustFired = false;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float launchForce = 50f;
    [SerializeField] private Transform projectileSpawner;

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
        hasJustFired = true;

        //temporary spawning code, use object pool later
        GameObject ball = Instantiate(cannonBallPrefab, projectileSpawner.position, projectileSpawner.rotation);
        if (ball.GetComponent<Rigidbody>())
            ball.GetComponent<Rigidbody>().AddForce(projectileSpawner.forward * launchForce, ForceMode.VelocityChange); //implement a range calculation later using the salvaged steel artillery code

        Invoke("CoolDown", fireRate);
        Debug.Log("cannon fired @ " + Time.time);
    }

    private void CoolDown()
    {
        hasJustFired = false;
    }
}
