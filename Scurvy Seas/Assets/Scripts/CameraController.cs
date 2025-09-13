using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Vector3 offset;
    public Transform followThis;
    public float moveSpeed = 7f;

    void FixedUpdate()
    {
        Vector3 targetPos = followThis.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
