using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Vector3 offset;
    public Transform followThis;
    public float moveSpeed = 7f;

    public float radiusClamp = 50f;

    public float maxYOffset;
    public float minYOffset;

    void LateUpdate()
    {
        Vector3 targetPos = followThis.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            offset.y -= scroll * 7f;
            offset.y = Mathf.Clamp(offset.y, minYOffset, maxYOffset);
        }

        Vector3 moveInput;
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.z = Input.GetAxis("Vertical");

        Vector3 newOffset = new Vector3(moveInput.x, 0, moveInput.z);
        
        offset += newOffset.normalized * moveSpeed * Time.deltaTime;
        offset.x = Mathf.Clamp(offset.x, -radiusClamp, radiusClamp);
        offset.z = Mathf.Clamp(offset.z, -radiusClamp, radiusClamp);
    }
}
