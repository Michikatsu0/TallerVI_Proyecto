using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;
    public float transitionDelay;

    private int sceneIndex;
    private bool onContinueButton;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Instance = this;
    }

    void Update()
    {
        if (onContinueButton)
        {
            StartCoroutine(nameof(TransitionNextScene));
        }
    }

    private IEnumerator TransitionNextScene()
    {

        yield return new WaitForSeconds(transitionDelay);
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        yield return null;
    }

    public void ContinueButton()
    {
        onContinueButton = true;
        Invoke(nameof(ContinueButton), 0.1f);
    }

    private void ResetContinueButton()
    {
        onContinueButton = false;
    }

    public void LoadingScene(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
    }

}
