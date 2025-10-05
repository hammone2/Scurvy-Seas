using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    private bool isColliding = false;
    private Transform collidingWith;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CellCollider"))
        {
            isColliding = true;
            collidingWith = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CellCollider"))
        {
            isColliding = false;
            collidingWith = null;
        }
    }

    public bool IsColliding()
    {
        return isColliding;
    }

    public Transform GetColliderPosition()
    {
        if (isColliding)
            return collidingWith;
        return null;
    }
}
