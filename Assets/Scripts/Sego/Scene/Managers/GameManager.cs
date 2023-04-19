using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndexes { TITLE, ZONE, LOADING,PROTOTYPING }
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE, LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



}
