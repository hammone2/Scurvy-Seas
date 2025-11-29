using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class CameraShake : MonoBehaviour
{
    private float shakeMagnitude = 0f;
    [SerializeField] private float maxMagnitude = 10f;
    [SerializeField] float maxOffset = 15f; // max distance allowed from anchor per axis
    [SerializeField] private float shakeFalloff = 0.01f;
    private Vector3 initialPos;

    void Awake()
    {
        initialPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeMagnitude > 0)
            shakeMagnitude -= shakeFalloff;
        if (shakeMagnitude < 0)
            shakeMagnitude = 0;


        transform.localPosition += Random.insideUnitSphere * shakeMagnitude;
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPos, 10f * Time.deltaTime);

        transform.localPosition = new Vector3(

            Mathf.Clamp(transform.localPosition.x, initialPos.x - maxOffset, initialPos.x + maxOffset),
            Mathf.Clamp(transform.localPosition.y, initialPos.y - maxOffset, initialPos.y + maxOffset),
            Mathf.Clamp(transform.localPosition.z, initialPos.z - maxOffset, initialPos.z + maxOffset)

            );
    }

    public void ScreenShake(float magnitude)
    {
        if (magnitude > maxMagnitude)
            magnitude = maxMagnitude;
        
        shakeMagnitude = magnitude;
        //HUD.instance.ScreenShake(magnitude);
    }
}
