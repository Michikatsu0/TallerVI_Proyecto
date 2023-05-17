using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string upgradeName;
    [SerializeField] int upgradeTarget;
    [SerializeField] int upgradePrice;
    int numberOfUpgrade;
    void Start()
    {
        upgradeName = PlayerPrefs.GetString("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDestroy() { }
    public void StoreBuy()
    {
        
    }
}
