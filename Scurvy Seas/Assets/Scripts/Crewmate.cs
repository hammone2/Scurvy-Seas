using UnityEngine;
using UnityEngine.AI;

public class Crewmate : MonoBehaviour
{
    [SerializeField] private GameObject navPoint;
    private Transform navTarget;
    private NavMeshAgent agent;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent.enabled)
        {
            if (navTarget != null)
                agent.destination = navTarget.position;

            //disable the agent once we reach the target
            if (agent.remainingDistance <= agent.stoppingDistance)
                agent.enabled = false; //using enable as opposed to isStopped so the agent wont slide around on the deck
        }
    }

    public void SetNavDestination(Vector3 endPoint)
    {
        if (!agent.enabled)  //using enable as opposed to isStopped so the agent wont slide around on the deck
            agent.enabled = true;

        navPoint.transform.position = endPoint;
        navTarget = navPoint.transform;
        agent.SetDestination(navTarget.position);
    }

    public void ReparentNavPoint(Transform newParent)
    {
        navPoint.transform.SetParent(newParent);
        SetNavDestination(navPoint.transform.position);
    }
}
