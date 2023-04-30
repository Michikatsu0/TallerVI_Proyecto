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

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        Instance = this;
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

        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        yield return null;
    }
}
