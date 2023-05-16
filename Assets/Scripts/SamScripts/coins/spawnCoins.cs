using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnCoins : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject cosaRaraMuyRara;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator lalal()
    {
       yield return new WaitForSeconds(3);
        cosaRaraMuyRara.GetComponent<coinBehavour>().rarozongo = 3;

    }
}
