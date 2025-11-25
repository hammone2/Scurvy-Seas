using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform followThis;
    [SerializeField] private float moveSpeed = 0.77f;
    [SerializeField] private float scrollSpeed = 10f;

    [SerializeField] private float radiusClamp = 50f;

    [SerializeField] private float maxYOffset;
    [SerializeField] private float minYOffset;

    void LateUpdate()
    {
        //apply offset and clamp position
        offset.x = Mathf.Clamp(offset.x, -radiusClamp, radiusClamp);
        offset.z = Mathf.Clamp(offset.z, -radiusClamp, radiusClamp);
        Vector3 targetPos = followThis.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (PlayerManager.instance.GetIsInventoryOpen() == true)
            return;

        //Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            offset.y -= scroll * scrollSpeed;
            offset.y = Mathf.Clamp(offset.y, minYOffset, maxYOffset);
        }

        //old WASD movement
        /*Vector3 moveInput;
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.z = Input.GetAxis("Vertical");

        Vector3 newOffset = new Vector3(moveInput.x, 0, moveInput.z);
        
        offset += newOffset.normalized * moveSpeed * Time.deltaTime;*/


        //Click and drag
        if (Input.GetMouseButton(0))
        {
            Vector3 _x = transform.right;
            Vector3 _z = transform.forward;

            Vector3 move = (-_x * Input.GetAxis("Mouse X") - _z * Input.GetAxis("Mouse Y")) * moveSpeed;
            move.Normalize();
            offset += move;
        }
    }
}
