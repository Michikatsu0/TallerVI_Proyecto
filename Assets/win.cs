using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class win : MonoBehaviour
{
    //public GameObject winnCanvas;

    [SerializeField] private LevelMenu levelmenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject Target = collision.gameObject;
        if(Target.CompareTag("Player"))
        {
            levelmenu.win = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject Target = other.gameObject;
        if (Target.CompareTag("Player"))
        {
            levelmenu.win = true;
        }
    }
}
