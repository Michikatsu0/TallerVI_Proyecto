using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public static TitleManager Instance;
    public float transitionDelay;
    public int tutorial;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        tutorial = PlayerPrefs.GetInt("TutorialComplete");
        TransitionUIPanel.Instance.FadeIn();    
    }

    void Update()
    {
        OnTapScreenTitle();
    }

    public void OnTapScreenTitle()
    {
        if (Input.touchCount > 0 && tutorial == 0)
        {
            PlayerPrefs.SetInt("LoadingSceneIndexToLoad", 3);
            StartCoroutine(TransitionToNextScene((int)SceneIndexes.LOADING));
        }
        else if (Input.touchCount > 0 && tutorial != 0)
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
