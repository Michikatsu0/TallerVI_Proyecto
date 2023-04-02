using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cannonRotation : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        rotation1();
    }

    // Update is called once per frame
    private void rotation1()
    {
        transform.DORotate(new Vector3(-115,-90,0),1).OnComplete(rotation2);
    }
    void rotation2()
    {
        transform.DORotate(new Vector3(-90, -90, 0), 1).OnComplete(rotation1);
    }

}
