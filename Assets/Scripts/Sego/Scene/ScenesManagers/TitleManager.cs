using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{ 
    public static TitleManager Instance;

    [SerializeField] private TransitionUIPanel transitionUIPanel;
    [SerializeField] private float transitionDelay;
    [SerializeField] private int tutorial;
    private void Start()
    {
        tutorial = PlayerPrefs.GetInt("TutorialComplete");
        transitionUIPanel.FadeIn();    
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
        transitionUIPanel.FadeOut();
        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        yield return null;
    }
}
