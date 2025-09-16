using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Vector3 offset;
    public Transform followThis;
    public float moveSpeed = 7f;

    public float maxYOffset;
    public float minYOffset;

    void Update()
    {
        Vector3 targetPos = followThis.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            offset.y -= scroll * 7f;
            offset.y = Mathf.Clamp(offset.y, minYOffset, maxYOffset);
        }
    }
}
