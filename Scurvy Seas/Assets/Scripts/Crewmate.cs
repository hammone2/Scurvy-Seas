using UnityEngine;
using UnityEngine.AI;

public class Crewmate : MonoBehaviour
{
    [SerializeField] private GameObject navPoint;
    private Transform navTarget;
    private NavMeshAgent agent;

    [SerializeField] private GameObject rotated;
    [SerializeField] private float rotationSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing on " + gameObject.name);
        }

        // Ensure navPoint is assigned
        if (navPoint == null)
        {
            Debug.LogWarning("NavPoint is not assigned on " + gameObject.name);
        }
    }

    void Update()
    {
        if (agent != null)
        {
            //update target destination
            if (agent.enabled && navTarget != null)
            {
                agent.destination = navTarget.position; //change this later so its not being calculated every frame

                //rotate the character towards the target
                Vector3 targetDirection = navTarget.position - transform.position;
                targetDirection.y = 0;

                if (targetDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
                

            //disable the agent once we reach the target
            if (agent.enabled && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.enabled = false; //using enable as opposed to isStopped so the agent wont slide around on the deck
            }
        }
    }

    public void SetNavDestination(Vector3 endPoint)
    {
        if (navPoint == null || agent == null)
        {
            Debug.LogError("NavPoint is null in SetNavDestination.");
            return;
        }

        agent.enabled = true;
        
        Debug.Log("New position");
        navPoint.transform.position = endPoint;
        navTarget = navPoint.transform;
    }

    public void ReparentNavPoint(Transform newParent)
    {
        if (navPoint != null && newParent != null)
        {
            navPoint.transform.SetParent(newParent);
            SetNavDestination(navPoint.transform.position);
        }
        else
        {
            Debug.LogWarning("NavPoint or newParent is null in ReparentNavPoint.");
        }
    }
}
