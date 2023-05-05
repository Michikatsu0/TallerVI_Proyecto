using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance;
    public float transitionDelay;
    public bool tutorial = true;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TransitionUIPanel.Instance.FadeIn();    
    }

    void Update()
    {
        OnTapScreenTitle();
    }

    public void OnTapScreenTitle()
    {
        if (Input.touchCount > 0 && tutorial)
        {
            StartCoroutine(TransitionToNextScene((int)SceneIndexes.LOADING));
        }
        else if (Input.touchCount > 0 && !tutorial)
        {
            SceneManager.LoadSceneAsync((int)SceneIndexes.LOBBY);
        }
    }
    public IEnumerator TransitionToNextScene(int sceneIndex)
    {
        TransitionUIPanel.Instance.FadeOut();
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        yield return null;
    }
}
