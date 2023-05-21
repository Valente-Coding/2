using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        // Get a reference to the main camera's transform
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        // Make the object face the camera
        transform.LookAt(cameraTransform);
        transform.Rotate(0f, 180f, 0f); // Optional: Rotate the object by 180 degrees to face the camera properly
    }
}