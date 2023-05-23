using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbyManager : MonoBehaviour
{
    [SerializeField] Canvas LevelCanvas;
    [SerializeField] Canvas UpgradesCanvas;
    [SerializeField] Canvas ArmoryCanvas;
    [SerializeField] Canvas StatsCanvas;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        LevelCanvas.enabled = false;
        UpgradesCanvas.enabled = false;
        ArmoryCanvas.enabled = false;
        StatsCanvas.enabled = false;
    }
}
