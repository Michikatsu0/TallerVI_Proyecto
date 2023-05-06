using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerPrefsVariables : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("LoadingSceneIndexToLoad", 0); // 0,1,2,3,4,5...etc
        PlayerPrefs.SetInt("TutorialComplete", 0); // 0,1
        SaveData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveData() 
    {
        PlayerPrefs.Save();
    }
}