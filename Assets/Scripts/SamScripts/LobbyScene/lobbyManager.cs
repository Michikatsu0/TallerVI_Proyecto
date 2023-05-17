using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbyManager : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] GameObject laCamara;
    [SerializeField] Canvas Elcanvas;
    // Start is called before the first frame update
    void Start()
    {
        Elcanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
