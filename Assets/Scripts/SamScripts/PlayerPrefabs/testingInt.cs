using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testingInt : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.SetInt("testVariable", 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
