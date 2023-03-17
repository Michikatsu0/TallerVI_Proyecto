using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smooth = 0.4f, x = 0, y = 0, z = 0;

    Vector3 velocidadCamara = Vector3.zero;
    void Update()
    {
        Vector3 posicion = new Vector3();
        posicion.x = player.position.x + x;
        posicion.z = player.position.z + z;
        posicion.y = player.position.y + y;
        transform.position = Vector3.SmoothDamp(transform.position, posicion, ref velocidadCamara, smooth);
    }
}
