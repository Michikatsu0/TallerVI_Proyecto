using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.6f;
    private Vector3 rotation;

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", - Time.time * rotationSpeed);
        rotation = new Vector3(45f, 55f + rotationSpeed * Time.time, 0f);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
