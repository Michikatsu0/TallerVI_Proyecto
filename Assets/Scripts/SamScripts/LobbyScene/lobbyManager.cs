using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobbyManager : MonoBehaviour
{
    [SerializeField] Canvas LevelCanvas;
    [SerializeField] Canvas UpgradesCanvas;
    [SerializeField] Canvas ArmoryCanvas;
    [SerializeField] Canvas StatsCanvas;

    private CanvasGroup Lvls;
    private CanvasGroup upgrades;
    private CanvasGroup armory;
    private CanvasGroup Stats;

    // Start is called before the first frame update
    void Start()
    {

     
          DontDestroyOnLoad(gameObject);
          Lvls = LevelCanvas.GetComponent<CanvasGroup>();
          upgrades = UpgradesCanvas.GetComponent<CanvasGroup>();
          armory = ArmoryCanvas.GetComponent<CanvasGroup>();
          Stats = StatsCanvas.GetComponent<CanvasGroup>();

          Lvls.alpha = 0f;
          upgrades.alpha = 0f;
          armory.alpha = 0f;
          Stats.alpha = 0f;

        LevelCanvas.enabled = false;
        UpgradesCanvas.enabled = false;
        ArmoryCanvas.enabled = false;
        StatsCanvas.enabled = false;

    }
}
